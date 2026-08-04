using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;

namespace TiendaRopa.Repositorio.Repositorios.PedidosRep
{
    public class DetallePedidoRepositorio : IDetallePedidoRepositorio
    {
        private readonly AppDbContext _context;

        public DetallePedidoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetallesPedido>> GetAllAsync()
        {
            return await _context.DetallesPedidos
                                 .Include(d => d.Pedido)
                                 .Include(d => d.Producto)
                                 .ToListAsync();
        }

        public async Task<DetallesPedido?> GetByIdAsync(int id)
        {
            return await _context.DetallesPedidos.FindAsync(id);
        }

        public async Task AddAsync(DetallesPedido detalle)
        {
            await _context.DetallesPedidos.AddAsync(detalle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DetallesPedido detalle)
        {
            _context.DetallesPedidos.Update(detalle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var detalle = await GetByIdAsync(id);
            if (detalle != null)
            {
                _context.DetallesPedidos.Remove(detalle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
