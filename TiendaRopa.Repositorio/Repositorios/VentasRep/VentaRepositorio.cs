using Microsoft.EntityFrameworkCore;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Venta;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Repositorio.Repositorios.VentasRep;

public class VentaRepositorio : Repositorio<Venta>, IRepositorio<Venta>, IVentaRepositorio
{
    private readonly AppDbContext context;
    public VentaRepositorio(AppDbContext context) : base(context) => this.context = context;

    public async Task<List<VentaMostrarDTO>> GetHistorial(CanalVenta? canal = null, DateTime? desde = null, DateTime? hasta = null, int? clienteId = null)
    {
        var query = context.Ventas.AsNoTracking().Where(v => v.EstadoRegistro != EstadoRegistro.eliminado);
        if (canal.HasValue) query = query.Where(v => v.CanalDeVenta == canal.Value);
        if (clienteId.HasValue) query = query.Where(v => v.ClienteId == clienteId.Value);
        if (desde.HasValue) query = query.Where(v => v.FechaHora >= ComoUtc(desde.Value));
        if (hasta.HasValue) query = query.Where(v => v.FechaHora < ComoUtc(hasta.Value));

        var ventas = await query.OrderByDescending(v => v.FechaHora).ThenByDescending(v => v.Id)
            .Select(v => new VentaMostrarDTO
            {
                Id = v.Id, FechaHora = v.FechaHora, Canal = v.CanalDeVenta, Estado = v.Estado,
                Monto = v.Monto, ClienteId = v.ClienteId,
                Cliente = v.Cliente.Nombre + (v.Cliente.Apellido != null ? " " + v.Cliente.Apellido : "")
            }).ToListAsync();

        if (ventas.Count == 0)
            return ventas;

        var ventaIds = ventas.Select(v => v.Id).ToList();

        var pagos = await context.PagosVenta.AsNoTracking()
            .Where(p => ventaIds.Contains(p.VentaId))
            .Select(p => new
            {
                p.VentaId,
                Pago = new PagoVentaMostrarDTO
                {
                    PagoId = p.PagoId,
                    Metodo = p.Pago!.Tipo,
                    Monto = p.Monto
                }
            })
            .ToListAsync();

        var marcas = await context.DetallesVenta.AsNoTracking()
            .Where(d => ventaIds.Contains(d.VentaId))
            .Select(d => new
            {
                d.VentaId,
                Marca = d.Variante.ProductoColor!.Producto!.Marca != null
                    ? d.Variante.ProductoColor.Producto.Marca.NombreMarca
                    : "Sin Marca"
            })
            .ToListAsync();

        var montosCuentaCorriente = await context.MovimientosCuentaCorriente.AsNoTracking()
            .Where(m => ventaIds.Contains(m.VentaId ?? 0) && m.Tipo == TipoMovimientoCuentaCorriente.Cargo)
            .GroupBy(m => m.VentaId)
            .Select(g => new { VentaId = g.Key, Monto = g.Sum(m => m.Monto) })
            .ToListAsync();

        var cantidades = await context.DetallesVenta.AsNoTracking()
            .Where(d => ventaIds.Contains(d.VentaId))
            .GroupBy(d => d.VentaId)
            .Select(g => new { VentaId = g.Key, Cantidad = g.Sum(d => d.CantidadProducto) })
            .ToListAsync();

        var pagosPorVenta = pagos.GroupBy(p => p.VentaId).ToDictionary(g => g.Key, g => g.Select(p => p.Pago).ToList());
        var marcasPorVenta = marcas.GroupBy(m => m.VentaId).ToDictionary(g => g.Key, g => g.Select(m => m.Marca).Distinct().ToList());
        var montosPorVenta = montosCuentaCorriente.ToDictionary(g => g.VentaId!.Value, g => g.Monto);
        var cantidadesPorVenta = cantidades.ToDictionary(g => g.VentaId, g => g.Cantidad);

        foreach (var venta in ventas)
        {
            venta.Pagos = pagosPorVenta.GetValueOrDefault(venta.Id, new List<PagoVentaMostrarDTO>());
            venta.Marcas = marcasPorVenta.GetValueOrDefault(venta.Id, new List<string>());
            venta.MontoCuentaCorriente = montosPorVenta.GetValueOrDefault(venta.Id);
            venta.CantidadArticulos = cantidadesPorVenta.GetValueOrDefault(venta.Id);
        }

        return ventas;
    }

    public async Task<VentaDetalleDTO?> ObtenerById(int id)
    {
        return await context.Ventas.AsNoTracking().Where(v => v.Id == id && v.EstadoRegistro != EstadoRegistro.eliminado)
            .Select(v => new VentaDetalleDTO
            {
                Id = v.Id, FechaHora = v.FechaHora, Canal = v.CanalDeVenta, Estado = v.Estado, Monto = v.Monto,
                ClienteId = v.ClienteId, Cliente = v.Cliente.Nombre + (v.Cliente.Apellido != null ? " " + v.Cliente.Apellido : ""),
                Pagos = v.PagosVenta.Select(p => new PagoVentaMostrarDTO { PagoId = p.PagoId, Metodo = p.Pago!.Tipo, Monto = p.Monto }).ToList(),
                MontoCuentaCorriente = context.MovimientosCuentaCorriente.Where(m => m.VentaId == v.Id && m.Tipo == TipoMovimientoCuentaCorriente.Cargo).Select(m => (decimal?)m.Monto).FirstOrDefault() ?? 0m,
                Detalles = v.DetallesVenta.OrderBy(d => d.Id).Select(d => new DetalleVentaMostrarDTO
                {
                    Id = d.Id, VarianteId = d.VarianteId, CodVariante = d.Variante.CodVariante,
                    Producto = d.Variante.ProductoColor!.Producto!.NombreProducto,
                    Marca = d.Variante.ProductoColor!.Producto!.Marca != null ? d.Variante.ProductoColor.Producto.Marca.NombreMarca : "Sin Marca",
                    Color = d.Variante.ProductoColor!.Color!.NombreColor, Talle = d.Variante.Talle!.NombreTalle,
                    UrlImagen = d.Variante.ProductoColor!.UrlImagen, Cantidad = d.CantidadProducto,
                    PrecioUnitario = d.PrecioUnitario, Subtotal = d.Subtotal, StockActual = d.Variante.Stock
                }).ToList()
            }).FirstOrDefaultAsync();
    }

    public async Task<ResultadoVenta> Crear(VentaCrearDTO dto, CanalVenta canal, int? carritoId = null)
    {
        var error = await ValidarEncabezado(dto);
        if (error != null) return ResultadoVenta.Falla(error);
        var variantes = await CargarVariantes(dto.Detalles.Select(d => d.VarianteId).ToList());
        var venta = new Venta { FechaHora = DateTime.UtcNow, CanalDeVenta = canal, Estado = EstadosVenta.Completada, ClienteId = dto.ClienteId, CarritoId = carritoId, EstadoRegistro = EstadoRegistro.activo };
        decimal total = 0;
        foreach (var linea in dto.Detalles)
        {
            if (!variantes.TryGetValue(linea.VarianteId, out var variante)) return ResultadoVenta.Falla($"El artículo con ID {linea.VarianteId} no existe.");
            if (variante.EstadoRegistro != EstadoRegistro.activo) return ResultadoVenta.Falla($"El artículo {NombreVariante(variante)} no está activo.");
            if (linea.Cantidad > variante.Stock) return ResultadoVenta.Falla(MensajeStock(variante, variante.Stock, linea.Cantidad));
            var precio = linea.PrecioUnitario ?? variante.PrecioVenta;
            var subtotal = CalcularSubtotal(linea.Cantidad, precio);
            venta.DetallesVenta.Add(new DetalleVenta { VarianteId = variante.Id, CantidadProducto = linea.Cantidad, PrecioUnitario = precio, Subtotal = subtotal, EstadoRegistro = EstadoRegistro.activo });
            variante.Stock -= linea.Cantidad;
            total += subtotal;
        }
        venta.Monto = total;
        var pagos = ResolverPagos(dto, total);
        var cuenta = Math.Round(dto.MontoCuentaCorriente, 2, MidpointRounding.AwayFromZero);
        error = ValidarDistribucion(pagos, cuenta, total, canal == CanalVenta.Presencial);
        if (error != null) return ResultadoVenta.Falla(error);
        foreach (var pago in pagos) venta.PagosVenta.Add(new PagoVenta { PagoId = pago.PagoId, Monto = Math.Round(pago.Monto, 2, MidpointRounding.AwayFromZero), EstadoRegistro = EstadoRegistro.activo });
        if (dto.MontoCuentaCorriente < 0) return ResultadoVenta.Falla("El importe de cuenta corriente no puede ser negativo.");
        if (cuenta > 0)
        {
            var cliente = await context.Clientes.FirstAsync(c => c.Id == dto.ClienteId);
            cliente.Saldo -= cuenta;
            context.MovimientosCuentaCorriente.Add(new MovimientoCuentaCorriente { Fecha = DateTime.UtcNow, Tipo = TipoMovimientoCuentaCorriente.Cargo, Monto = cuenta, ClienteId = dto.ClienteId, Venta = venta, EstadoRegistro = EstadoRegistro.activo });
        }
        context.Ventas.Add(venta);
        await context.SaveChangesAsync();
        return ResultadoVenta.Ok(venta.Id);
    }

    public async Task<ResultadoVenta> Editar(int id, VentaCrearDTO dto)
    {
        var venta = await context.Ventas.Include(v => v.DetallesVenta).Include(v => v.PagosVenta)
            .FirstOrDefaultAsync(v => v.Id == id && v.EstadoRegistro != EstadoRegistro.eliminado);
        if (venta == null) return ResultadoVenta.NoExiste(id);
        if (venta.Estado == EstadosVenta.Anulada) return ResultadoVenta.Falla("No se puede editar una venta anulada.");
        if (venta.CanalDeVenta != CanalVenta.Presencial) return ResultadoVenta.Falla("Solo se pueden editar ventas presenciales.");
        var error = await ValidarEncabezado(dto);
        if (error != null) return ResultadoVenta.Falla(error);

        var anteriores = venta.DetallesVenta.ToDictionary(d => d.VarianteId);
        var variantes = await CargarVariantes(dto.Detalles.Select(d => d.VarianteId).Union(anteriores.Keys).ToList());
        foreach (var linea in dto.Detalles)
        {
            if (!variantes.TryGetValue(linea.VarianteId, out var variante)) return ResultadoVenta.Falla($"El artículo con ID {linea.VarianteId} no existe.");
            anteriores.TryGetValue(linea.VarianteId, out var anterior);
            if (anterior == null && variante.EstadoRegistro != EstadoRegistro.activo) return ResultadoVenta.Falla($"El artículo {NombreVariante(variante)} no está activo.");
            var disponible = variante.Stock + (anterior?.CantidadProducto ?? 0);
            if (linea.Cantidad > disponible) return ResultadoVenta.Falla(MensajeStock(variante, disponible, linea.Cantidad));
        }
        foreach (var anterior in anteriores.Values) variantes[anterior.VarianteId].Stock += anterior.CantidadProducto;
        var idsActuales = dto.Detalles.Select(d => d.VarianteId).ToHashSet();
        context.DetallesVenta.RemoveRange(venta.DetallesVenta.Where(d => !idsActuales.Contains(d.VarianteId)).ToList());
        decimal total = 0;
        foreach (var linea in dto.Detalles)
        {
            var variante = variantes[linea.VarianteId];
            variante.Stock -= linea.Cantidad;
            var precio = linea.PrecioUnitario ?? (anteriores.TryGetValue(linea.VarianteId, out var anterior) ? anterior.PrecioUnitario : variante.PrecioVenta);
            var subtotal = CalcularSubtotal(linea.Cantidad, precio);
            if (anteriores.TryGetValue(linea.VarianteId, out var existente))
            {
                existente.CantidadProducto = linea.Cantidad; existente.PrecioUnitario = precio; existente.Subtotal = subtotal;
            }
            else venta.DetallesVenta.Add(new DetalleVenta { VarianteId = variante.Id, CantidadProducto = linea.Cantidad, PrecioUnitario = precio, Subtotal = subtotal, EstadoRegistro = EstadoRegistro.activo });
            total += subtotal;
        }
        var pagos = ResolverPagos(dto, total);
        var cuentaNueva = Math.Round(dto.MontoCuentaCorriente, 2, MidpointRounding.AwayFromZero);
        error = ValidarDistribucion(pagos, cuentaNueva, total, true);
        if (error != null) return ResultadoVenta.Falla(error);
        var movimiento = await context.MovimientosCuentaCorriente.FirstOrDefaultAsync(m => m.VentaId == id && m.Tipo == TipoMovimientoCuentaCorriente.Cargo);
        var cuentaAnterior = movimiento?.Monto ?? 0m;
        var clienteAnteriorId = movimiento?.ClienteId;
        if (movimiento != null && clienteAnteriorId != dto.ClienteId)
        {
            var clienteAnterior = await context.Clientes.FirstAsync(c => c.Id == clienteAnteriorId);
            clienteAnterior.Saldo += cuentaAnterior;
            cuentaAnterior = 0m;
            clienteAnteriorId = null;
        }
        if (cuentaNueva > 0)
        {
            var cliente = await context.Clientes.FirstAsync(c => c.Id == dto.ClienteId);
            cliente.Saldo += cuentaAnterior - cuentaNueva;
            if (movimiento == null) context.MovimientosCuentaCorriente.Add(new MovimientoCuentaCorriente { Fecha = DateTime.UtcNow, Tipo = TipoMovimientoCuentaCorriente.Cargo, Monto = cuentaNueva, ClienteId = dto.ClienteId, VentaId = id, EstadoRegistro = EstadoRegistro.activo });
            else { movimiento.ClienteId = dto.ClienteId; movimiento.Monto = cuentaNueva; movimiento.Fecha = DateTime.UtcNow; }
        }
        else if (movimiento != null) context.MovimientosCuentaCorriente.Remove(movimiento);
        venta.ClienteId = dto.ClienteId; venta.Monto = total;
        context.PagosVenta.RemoveRange(venta.PagosVenta.ToList());
        venta.PagosVenta.Clear();
        foreach (var pago in pagos) venta.PagosVenta.Add(new PagoVenta { PagoId = pago.PagoId, Monto = pago.Monto, EstadoRegistro = EstadoRegistro.activo });
        await context.SaveChangesAsync();
        return ResultadoVenta.Ok(venta.Id);
    }

    public async Task<ResultadoVenta> Anular(int id)
    {
        var venta = await context.Ventas.Include(v => v.DetallesVenta).FirstOrDefaultAsync(v => v.Id == id && v.EstadoRegistro != EstadoRegistro.eliminado);
        if (venta == null) return ResultadoVenta.NoExiste(id);
        if (venta.Estado == EstadosVenta.Anulada) return ResultadoVenta.Falla("La venta ya está anulada.");
        if (venta.CanalDeVenta != CanalVenta.Presencial) return ResultadoVenta.Falla("Solo se pueden anular ventas presenciales.");
        var variantes = await CargarVariantes(venta.DetallesVenta.Select(d => d.VarianteId).ToList());
        foreach (var detalle in venta.DetallesVenta) if (variantes.TryGetValue(detalle.VarianteId, out var variante)) variante.Stock += detalle.CantidadProducto;
        var cargo = await context.MovimientosCuentaCorriente.FirstOrDefaultAsync(m => m.VentaId == id && m.Tipo == TipoMovimientoCuentaCorriente.Cargo);
        if (cargo != null) { var cliente = await context.Clientes.FirstAsync(c => c.Id == cargo.ClienteId); cliente.Saldo += cargo.Monto; context.MovimientosCuentaCorriente.Remove(cargo); }
        venta.Estado = EstadosVenta.Anulada;
        await context.SaveChangesAsync();
        return ResultadoVenta.Ok(venta.Id);
    }

    private async Task<string?> ValidarEncabezado(VentaCrearDTO dto)
    {
        if (dto.Detalles == null || dto.Detalles.Count == 0) return "La venta debe tener al menos un artículo.";
        if (dto.Detalles.Any(d => d.Cantidad < 1)) return "La cantidad de cada artículo debe ser mayor a 0.";
        if (dto.Detalles.Any(d => d.PrecioUnitario < 0)) return "El precio unitario no puede ser negativo.";
        if (dto.Detalles.GroupBy(d => d.VarianteId).Any(g => g.Count() > 1)) return "Un mismo artículo no puede repetirse; sume las cantidades en un solo renglón.";
        if (!await context.Clientes.AnyAsync(c => c.Id == dto.ClienteId)) return "El cliente seleccionado no existe.";
        foreach (var pago in ResolverPagos(dto, 0))
            if (pago.PagoId <= 0 || !await context.Pagos.AnyAsync(p => p.Id == pago.PagoId && p.EstadoRegistro == EstadoRegistro.activo)) return "Uno de los métodos de pago seleccionados no existe o está inactivo.";
        return null;
    }

    private static List<PagoVentaCrearDTO> ResolverPagos(VentaCrearDTO dto, decimal total)
    {
        if (dto.Pagos?.Count > 0) return dto.Pagos;
        if (dto.PagoId.HasValue) return new() { new PagoVentaCrearDTO { PagoId = dto.PagoId.Value, Monto = total - dto.MontoCuentaCorriente } };
        return new();
    }
    private static string? ValidarDistribucion(List<PagoVentaCrearDTO> pagos, decimal cuenta, decimal total, bool requerida)
    {
        if (cuenta < 0 || cuenta > total) return "El importe de cuenta corriente debe estar entre cero y el total.";
        if (pagos.Any(p => p.PagoId <= 0 || p.Monto <= 0)) return "Cada medio de pago debe tener un importe mayor a cero.";
        if (pagos.GroupBy(p => p.PagoId).Any(g => g.Count() > 1)) return "No repita un medio de pago.";
        if (requerida && Math.Round(pagos.Sum(p => p.Monto) + cuenta, 2, MidpointRounding.AwayFromZero) != Math.Round(total, 2, MidpointRounding.AwayFromZero)) return "Los importes de pago y cuenta corriente deben coincidir con el total.";
        return null;
    }
    private async Task<Dictionary<int, Variante>> CargarVariantes(List<int> ids) => await context.Variantes
        .Include(v => v.ProductoColor).ThenInclude(pc => pc!.Producto).Include(v => v.ProductoColor).ThenInclude(pc => pc!.Color).Include(v => v.Talle)
        .Where(v => ids.Contains(v.Id)).ToDictionaryAsync(v => v.Id);
    private static decimal CalcularSubtotal(int cantidad, decimal precio) => Math.Round(cantidad * precio, 2, MidpointRounding.AwayFromZero);
    private static string NombreVariante(Variante v) => $"{v.ProductoColor?.Producto?.NombreProducto ?? "Artículo"} - {v.ProductoColor?.Color?.NombreColor} (talle {v.Talle?.NombreTalle})";
    private static string MensajeStock(Variante v, int disponible, int pedido) => $"Stock insuficiente para {NombreVariante(v)}: disponible {disponible}, solicitado {pedido}.";
    private static DateTime ComoUtc(DateTime fecha) => fecha.Kind switch { DateTimeKind.Utc => fecha, DateTimeKind.Local => fecha.ToUniversalTime(), _ => DateTime.SpecifyKind(fecha, DateTimeKind.Utc) };
}
