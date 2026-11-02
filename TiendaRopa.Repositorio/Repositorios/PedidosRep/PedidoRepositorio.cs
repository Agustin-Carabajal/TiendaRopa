using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;

namespace TiendaRopa.Repositorio.Repositorios.PedidosRep
{
    public class PedidoRepositorio : IPedidoRepositorio
    {
        private readonly AppDbContext _context;

        public PedidoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos
                                 .Include(p => p.Proveedor)
                                 .ToListAsync();
        }

        public async Task<Pedido?> GetByIdAsync(int id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task AddAsync(Pedido pedido)
        {
            try
            {
                pedido.Proveedor = null;

                if (string.IsNullOrWhiteSpace(pedido.FacturaPedidos))
                {
                    pedido.FacturaPedidos = $"PED-{DateTime.UtcNow:yyyyMMdd-HHmm}";
                }

                pedido.FechaDePedido = DateTime.SpecifyKind(pedido.FechaDePedido, DateTimeKind.Utc);

                if (pedido.FechaDeEntrega.HasValue)
                {
                    pedido.FechaDeEntrega = DateTime.SpecifyKind(pedido.FechaDeEntrega.Value, DateTimeKind.Utc);
                }

                await _context.Pedidos.AddAsync(pedido);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception($"Error al guardar el pedido en la base de datos: {innerMsg}", ex);
            }
        }

        public async Task UpdateAsync(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pedido = await GetByIdAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }

        // <-- Nueva implementación requerida por la interfaz
        public async Task<IEnumerable<DetallesPedido>> GetDetallesByPedidoIdAsync(int pedidoId)
        {
            return await _context.DetallesPedidos
                                 .Where(d => d.PedidoId == pedidoId)
                                 .ToListAsync();
        }
    }
}
