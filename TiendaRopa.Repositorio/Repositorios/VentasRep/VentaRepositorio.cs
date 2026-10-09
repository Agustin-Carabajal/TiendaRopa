using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Cliente;
using TiendaRopa.Shared.DTO.Producto_y_mas;
using TiendaRopa.Shared.DTO.Venta;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Repositorio.Repositorios.VentasRep
{
    public class VentaRepositorio : Repositorio<Venta>, IRepositorio<Venta>, IVentaRepositorio
    {
        private readonly AppDbContext context;

        public VentaRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        // =====================================================================
        // CONSULTAS
        // =====================================================================

        public async Task<List<VentaMostrarDTO>> GetHistorial(
            CanalVenta? canal = null, DateTime? desde = null, DateTime? hasta = null, int? clienteId = null)
        {
            var query = context.Set<Venta>()
                .AsNoTracking()
                .Where(v => v.EstadoRegistro != EstadoRegistro.eliminado);

            if (canal.HasValue)
                query = query.Where(v => v.CanalDeVenta == canal.Value);

            if (clienteId.HasValue)
                query = query.Where(v => v.ClienteId == clienteId.Value);

            if (desde.HasValue)
            {
                var desdeUtc = ComoUtc(desde.Value);
                query = query.Where(v => v.FechaHora >= desdeUtc);
            }

            if (hasta.HasValue)
            {
                // "hasta" es un límite superior exclusivo (la pantalla manda el inicio del día siguiente).
                var hastaUtc = ComoUtc(hasta.Value);
                query = query.Where(v => v.FechaHora < hastaUtc);
            }

            return await query
                .OrderByDescending(v => v.FechaHora)
                .ThenByDescending(v => v.Id)
                .Select(v => new VentaMostrarDTO
                {
                    Id = v.Id,
                    FechaHora = v.FechaHora,
                    Canal = v.CanalDeVenta,
                    Estado = v.Estado,
                    Monto = v.Monto,
                    ClienteId = v.ClienteId,
                    Cliente = v.Cliente.Nombre + (v.Cliente.Apellido != null ? " " + v.Cliente.Apellido : ""),
                    MetodoPago = v.PagosVenta.Select(p => p.Pago!.Tipo).FirstOrDefault(),
                    CantidadArticulos = v.DetallesVenta.Sum(d => d.CantidadProducto)
                })
                .ToListAsync();
        }

        public async Task<VentaDetalleDTO?> ObtenerById(int id)
        {
            return await context.Set<Venta>()
                .AsNoTracking()
                .Where(v => v.Id == id && v.EstadoRegistro != EstadoRegistro.eliminado)
                .Select(v => new VentaDetalleDTO
                {
                    Id = v.Id,
                    FechaHora = v.FechaHora,
                    Canal = v.CanalDeVenta,
                    Estado = v.Estado,
                    Monto = v.Monto,
                    ClienteId = v.ClienteId,
                    Cliente = v.Cliente.Nombre + (v.Cliente.Apellido != null ? " " + v.Cliente.Apellido : ""),
                    PagoId = v.PagosVenta.Select(p => (int?)p.PagoId).FirstOrDefault(),
                    MetodoPago = v.PagosVenta.Select(p => p.Pago!.Tipo).FirstOrDefault(),
                    Detalles = v.DetallesVenta
                        .OrderBy(d => d.Id)
                        .Select(d => new DetalleVentaMostrarDTO
                        {
                            Id = d.Id,
                            VarianteId = d.VarianteId,
                            CodVariante = d.Variante.CodVariante,
                            Producto = d.Variante.ProductoColor!.Producto!.NombreProducto,
                            Color = d.Variante.ProductoColor!.Color!.NombreColor,
                            Talle = d.Variante.Talle!.NombreTalle,
                            UrlImagen = d.Variante.ProductoColor!.UrlImagen,
                            Cantidad = d.CantidadProducto,
                            PrecioUnitario = d.PrecioUnitario,
                            Subtotal = d.Subtotal,
                            StockActual = d.Variante.Stock
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<ClienteMostrarDTO>> GetClientesActivos()
        {
            return await context.Clientes
                .AsNoTracking()
                .Where(c => c.EstadoRegistro == EstadoRegistro.activo)
                .OrderBy(c => c.Apellido).ThenBy(c => c.Nombre)
                .Select(c => new ClienteMostrarDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Dni = c.Dni
                })
                .ToListAsync();
        }

        public async Task<List<VarianteMostrarDTO>> GetVariantesDisponibles()
        {
            return await context.Variantes
                .AsNoTracking()
                .Where(v => v.EstadoRegistro == EstadoRegistro.activo && v.Stock > 0)
                .OrderBy(v => v.ProductoColor!.Producto!.NombreProducto)
                .ThenBy(v => v.ProductoColor!.Color!.NombreColor)
                .ThenBy(v => v.TalleId)
                .Select(v => new VarianteMostrarDTO
                {
                    Id = v.Id,
                    CodVariante = v.CodVariante,
                    ProductoId = v.ProductoColor!.ProductoId,
                    ProductoColor = v.ProductoColor!.Producto!.NombreProducto + " - " + v.ProductoColor.Color!.NombreColor,
                    Talle = v.Talle!.NombreTalle,
                    Stock = v.Stock,
                    PrecioVenta = v.PrecioVenta,
                    Estado = v.EstadoRegistro,
                    UrlImagen = v.ProductoColor.UrlImagen,
                    ColorId = v.ProductoColor.ColorId,
                    TalleId = v.TalleId
                })
                .ToListAsync();
        }

        public async Task<List<PagoMostrarDTO>> GetMetodosPago()
        {
            return await context.Set<Pago>()
                .AsNoTracking()
                .Where(p => p.EstadoRegistro == EstadoRegistro.activo)
                .OrderBy(p => p.Tipo)
                .Select(p => new PagoMostrarDTO { Id = p.Id, Tipo = p.Tipo })
                .ToListAsync();
        }

        // =====================================================================
        // COMANDOS
        // =====================================================================

        public async Task<ResultadoVenta> Crear(VentaCrearDTO dto, CanalVenta canal, int? carritoId = null)
        {
            var error = await ValidarEncabezado(dto);
            if (error != null) return ResultadoVenta.Falla(error);

            var variantes = await CargarVariantes(dto.Detalles.Select(d => d.VarianteId).ToList());

            var venta = new Venta
            {
                FechaHora = DateTime.UtcNow, // timestamptz de Npgsql exige DateTime en UTC
                CanalDeVenta = canal,
                Estado = EstadosVenta.Completada,
                ClienteId = dto.ClienteId,
                CarritoId = carritoId,
                EstadoRegistro = EstadoRegistro.activo
            };

            decimal total = 0;
            foreach (var linea in dto.Detalles)
            {
                if (!variantes.TryGetValue(linea.VarianteId, out var variante))
                    return ResultadoVenta.Falla($"El artículo con ID {linea.VarianteId} no existe.");

                if (variante.EstadoRegistro != EstadoRegistro.activo)
                    return ResultadoVenta.Falla($"El artículo {NombreVariante(variante)} no está activo.");

                if (linea.Cantidad > variante.Stock)
                    return ResultadoVenta.Falla(MensajeStock(variante, variante.Stock, linea.Cantidad));

                var precio = linea.PrecioUnitario ?? variante.PrecioVenta;
                var subtotal = CalcularSubtotal(linea.Cantidad, precio);

                venta.DetallesVenta.Add(new DetalleVenta
                {
                    VarianteId = variante.Id,
                    CantidadProducto = linea.Cantidad,
                    PrecioUnitario = precio,
                    Subtotal = subtotal,
                    EstadoRegistro = EstadoRegistro.activo
                });

                variante.Stock -= linea.Cantidad;
                total += subtotal;
            }

            venta.Monto = total;

            if (dto.PagoId.HasValue)
            {
                venta.PagosVenta.Add(new PagoVenta
                {
                    PagoId = dto.PagoId.Value,
                    EstadoRegistro = EstadoRegistro.activo
                });
            }

            // Un único SaveChanges = una única transacción: venta, detalles, pago y stock se guardan juntos o no se guarda nada.
            context.Set<Venta>().Add(venta);
            await context.SaveChangesAsync();

            return ResultadoVenta.Ok(venta.Id);
        }

        public async Task<ResultadoVenta> Editar(int id, VentaCrearDTO dto)
        {
            var venta = await context.Set<Venta>()
                .Include(v => v.DetallesVenta)
                .Include(v => v.PagosVenta)
                .FirstOrDefaultAsync(v => v.Id == id && v.EstadoRegistro != EstadoRegistro.eliminado);

            if (venta == null) return ResultadoVenta.NoExiste(id);
            if (venta.Estado == EstadosVenta.Anulada)
                return ResultadoVenta.Falla("No se puede editar una venta anulada.");
            if (venta.CanalDeVenta != CanalVenta.Presencial)
                return ResultadoVenta.Falla("Solo se pueden editar ventas presenciales.");

            var error = await ValidarEncabezado(dto);
            if (error != null) return ResultadoVenta.Falla(error);

            var anteriores = venta.DetallesVenta.ToDictionary(d => d.VarianteId);
            var idsVariantes = dto.Detalles.Select(d => d.VarianteId)
                .Union(anteriores.Keys)
                .ToList();
            var variantes = await CargarVariantes(idsVariantes);

            // 1) Validar todo antes de tocar nada. Lo que ya estaba vendido cuenta como disponible.
            foreach (var linea in dto.Detalles)
            {
                if (!variantes.TryGetValue(linea.VarianteId, out var variante))
                    return ResultadoVenta.Falla($"El artículo con ID {linea.VarianteId} no existe.");

                anteriores.TryGetValue(linea.VarianteId, out var anterior);

                // Un artículo ya vendido puede quedar aunque se haya dado de baja; uno nuevo no.
                if (anterior == null && variante.EstadoRegistro != EstadoRegistro.activo)
                    return ResultadoVenta.Falla($"El artículo {NombreVariante(variante)} no está activo.");

                var disponible = variante.Stock + (anterior?.CantidadProducto ?? 0);
                if (linea.Cantidad > disponible)
                    return ResultadoVenta.Falla(MensajeStock(variante, disponible, linea.Cantidad));
            }

            // 2) Devolver al stock todo lo que figuraba vendido...
            foreach (var anterior in anteriores.Values)
                variantes[anterior.VarianteId].Stock += anterior.CantidadProducto;

            // 3) ...quitar los renglones que ya no están...
            var idsNuevos = dto.Detalles.Select(d => d.VarianteId).ToHashSet();
            var quitados = venta.DetallesVenta.Where(d => !idsNuevos.Contains(d.VarianteId)).ToList();
            context.Set<DetalleVenta>().RemoveRange(quitados);

            // 4) ...y aplicar el nuevo estado de cada renglón descontando stock.
            decimal total = 0;
            foreach (var linea in dto.Detalles)
            {
                var variante = variantes[linea.VarianteId];
                variante.Stock -= linea.Cantidad;

                decimal subtotal;
                if (anteriores.TryGetValue(linea.VarianteId, out var detalle))
                {
                    // Si no mandan precio se conserva el que se cobró originalmente.
                    detalle.CantidadProducto = linea.Cantidad;
                    detalle.PrecioUnitario = linea.PrecioUnitario ?? detalle.PrecioUnitario;
                    detalle.Subtotal = CalcularSubtotal(detalle.CantidadProducto, detalle.PrecioUnitario);
                    subtotal = detalle.Subtotal;
                }
                else
                {
                    var precio = linea.PrecioUnitario ?? variante.PrecioVenta;
                    subtotal = CalcularSubtotal(linea.Cantidad, precio);
                    venta.DetallesVenta.Add(new DetalleVenta
                    {
                        VarianteId = variante.Id,
                        CantidadProducto = linea.Cantidad,
                        PrecioUnitario = precio,
                        Subtotal = subtotal,
                        EstadoRegistro = EstadoRegistro.activo
                    });
                }

                total += subtotal;
            }

            venta.ClienteId = dto.ClienteId;
            venta.Monto = total;

            // Método de pago: se reemplaza solo si cambió.
            if (venta.PagosVenta.FirstOrDefault()?.PagoId != dto.PagoId)
            {
                context.Set<PagoVenta>().RemoveRange(venta.PagosVenta.ToList());
                if (dto.PagoId.HasValue)
                {
                    venta.PagosVenta.Add(new PagoVenta
                    {
                        PagoId = dto.PagoId.Value,
                        EstadoRegistro = EstadoRegistro.activo
                    });
                }
            }

            await context.SaveChangesAsync();
            return ResultadoVenta.Ok(venta.Id);
        }

        public async Task<ResultadoVenta> Anular(int id)
        {
            var venta = await context.Set<Venta>()
                .Include(v => v.DetallesVenta)
                .FirstOrDefaultAsync(v => v.Id == id && v.EstadoRegistro != EstadoRegistro.eliminado);

            if (venta == null) return ResultadoVenta.NoExiste(id);
            if (venta.Estado == EstadosVenta.Anulada)
                return ResultadoVenta.Falla("La venta ya está anulada.");
            if (venta.CanalDeVenta != CanalVenta.Presencial)
                return ResultadoVenta.Falla("Solo se pueden anular ventas presenciales.");

            var variantes = await CargarVariantes(venta.DetallesVenta.Select(d => d.VarianteId).ToList());
            foreach (var detalle in venta.DetallesVenta)
            {
                if (variantes.TryGetValue(detalle.VarianteId, out var variante))
                    variante.Stock += detalle.CantidadProducto;
            }

            venta.Estado = EstadosVenta.Anulada;

            await context.SaveChangesAsync();
            return ResultadoVenta.Ok(venta.Id);
        }

        // =====================================================================
        // AUXILIARES
        // =====================================================================

        /// <summary>Valida cliente, método de pago y renglones. Devuelve el mensaje de error o null si está todo bien.</summary>
        private async Task<string?> ValidarEncabezado(VentaCrearDTO dto)
        {
            if (dto.Detalles == null || dto.Detalles.Count == 0)
                return "La venta debe tener al menos un artículo.";

            if (dto.Detalles.Any(d => d.Cantidad < 1))
                return "La cantidad de cada artículo debe ser mayor a 0.";

            if (dto.Detalles.Any(d => d.PrecioUnitario < 0))
                return "El precio unitario no puede ser negativo.";

            if (dto.Detalles.GroupBy(d => d.VarianteId).Any(g => g.Count() > 1))
                return "Un mismo artículo no puede repetirse en la venta; sume las cantidades en un solo renglón.";

            if (!await context.Clientes.AnyAsync(c => c.Id == dto.ClienteId))
                return "El cliente seleccionado no existe.";

            if (dto.PagoId.HasValue && !await context.Set<Pago>().AnyAsync(p => p.Id == dto.PagoId.Value))
                return "El método de pago seleccionado no existe.";

            return null;
        }

        /// <summary>Carga las variantes con seguimiento (para poder modificar Stock) y con los datos para armar mensajes.</summary>
        private async Task<Dictionary<int, Variante>> CargarVariantes(List<int> ids)
        {
            return await context.Variantes
                .Include(v => v.ProductoColor).ThenInclude(pc => pc!.Producto)
                .Include(v => v.ProductoColor).ThenInclude(pc => pc!.Color)
                .Include(v => v.Talle)
                .Where(v => ids.Contains(v.Id))
                .ToDictionaryAsync(v => v.Id);
        }

        private static decimal CalcularSubtotal(int cantidad, decimal precio)
            => Math.Round(cantidad * precio, 2, MidpointRounding.AwayFromZero);

        private static string NombreVariante(Variante v)
        {
            var producto = v.ProductoColor?.Producto?.NombreProducto ?? "Artículo";
            var color = v.ProductoColor?.Color?.NombreColor;
            var talle = v.Talle?.NombreTalle;

            var nombre = color != null ? $"{producto} - {color}" : producto;
            return talle != null ? $"{nombre} (talle {talle})" : nombre;
        }

        private static string MensajeStock(Variante v, int disponible, int pedido)
            => $"Stock insuficiente para {NombreVariante(v)}: disponible {disponible}, solicitado {pedido}.";

        private static DateTime ComoUtc(DateTime fecha) => fecha.Kind switch
        {
            DateTimeKind.Utc => fecha,
            DateTimeKind.Local => fecha.ToUniversalTime(),
            _ => DateTime.SpecifyKind(fecha, DateTimeKind.Utc)
        };
    }
}
