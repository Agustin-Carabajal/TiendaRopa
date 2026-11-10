using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Shared.DTO.Proveedor;

namespace TiendaRopa.Repositorio.Repositorios.PedidosRep
{
    public class RecepcionRepositorio : IRecepcionRepositorio
    {
        private readonly AppDbContext _context;

        public RecepcionRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recepcion>> GetAllAsync()
        {
            return await _context.Recepciones.ToListAsync();
        }

        public async Task<Recepcion?> GetByIdAsync(int id)
        {
            return await _context.Recepciones.FindAsync(id);
        }

        public async Task<Recepcion> AddRecepcionAsync(Recepcion recepcion)
        {
            await _context.Recepciones.AddAsync(recepcion);
            await _context.SaveChangesAsync();
            return recepcion;
        }

        public async Task AddDetalleRecepcionAsync(DetallesRecepcion detalle)
        {
            var costoRecibidoAntes = await _context.DetallesRecepciones
                .Where(dr => dr.PedidoId == detalle.PedidoId)
                .SumAsync(dr => (decimal?)(dr.PrecioUnitario * dr.CantidadRecibida)) ?? 0m;

            await _context.DetallesRecepciones.AddAsync(detalle);

            var variante = await _context.Variantes.FindAsync(detalle.VarianteId);
            if (variante != null)
            {
                variante.Stock += detalle.CantidadRecibida;
                variante.PrecioVenta = detalle.PrecioUnitario + detalle.PrecioEnvio + detalle.Ganancia;
                _context.Variantes.Update(variante);
            }

            var pedido = await _context.Pedidos.FindAsync(detalle.PedidoId);
            if (pedido != null)
                pedido.TotalPedidos = costoRecibidoAntes + detalle.PrecioUnitario * detalle.CantidadRecibida;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ItemPedidoPendienteDTO>> GetItemsPendientesPorPedidoAsync(int pedidoId)
        {
            var detallesPedido = await _context.DetallesPedidos
                .Include(d => d.Variante)
                    .ThenInclude(v => v!.ProductoColor)
                        .ThenInclude(pc => pc!.Producto)
                .Include(d => d.Variante)
                    .ThenInclude(v => v!.ProductoColor)
                        .ThenInclude(pc => pc!.Color)
                .Include(d => d.Variante)
                    .ThenInclude(v => v!.Talle)
                .Where(d => d.PedidoId == pedidoId)
                .ToListAsync();

            var recepcionesPrevias = await _context.DetallesRecepciones
                .Where(dr => dr.PedidoId == pedidoId)
                .ToListAsync();
            var variantesDelPedido = detallesPedido.Select(d => d.VarianteId).Distinct().ToList();
            var costosRecepcionados = await _context.DetallesRecepciones
                .Where(dr => variantesDelPedido.Contains(dr.VarianteId))
                .OrderBy(dr => dr.Id)
                .ToListAsync();

            var itemsPendientes = detallesPedido.Select(dp =>
            {
                int yaRecibido = recepcionesPrevias
                    .Where(dr => dr.VarianteId == dp.VarianteId)
                    .Sum(dr => dr.CantidadRecibida);
                var ultimoCosto = costosRecepcionados.LastOrDefault(dr => dr.VarianteId == dp.VarianteId);

                return new ItemPedidoPendienteDTO
                {
                    PedidoId = dp.PedidoId,
                    VarianteId = dp.VarianteId,
                    NombreProducto = dp.Variante == null
                        ? "Variante no disponible"
                        : $"{dp.Variante.ProductoColor?.Producto?.NombreProducto ?? "Producto"} - {dp.Variante.ProductoColor?.Color?.NombreColor ?? "Color sin asignar"} - Talle {dp.Variante.Talle?.NombreTalle ?? "Sin talle"} - SKU {dp.Variante.CodVariante}",
                    CantidadPedida = dp.Cant_prod_Pedido,
                    CantidadYaRecibida = yaRecibido,
                    PrecioUnitario = ultimoCosto?.PrecioUnitario ?? 0m,
                    CantidadARecibirAhora = Math.Max(0, dp.Cant_prod_Pedido - yaRecibido)
                };
            }).ToList();

            return itemsPendientes;
        }
    }
}
