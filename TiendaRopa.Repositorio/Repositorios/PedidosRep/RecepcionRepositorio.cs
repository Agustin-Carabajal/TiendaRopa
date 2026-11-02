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
            await _context.DetallesRecepciones.AddAsync(detalle);

            var variante = await _context.Variantes.FindAsync(detalle.VarianteId);
            if (variante != null)
            {
                variante.Stock += detalle.CantidadRecibida;
                _context.Variantes.Update(variante);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ItemPedidoPendienteDTO>> GetItemsPendientesPorPedidoAsync(int pedidoId)
        {
            var detallesPedido = await _context.DetallesPedidos
                .Include(d => d.Variante)
                .Where(d => d.PedidoId == pedidoId)
                .ToListAsync();

            var recepcionesPrevias = await _context.DetallesRecepciones
                .Where(dr => dr.PedidoId == pedidoId)
                .ToListAsync();

            var itemsPendientes = detallesPedido.Select(dp =>
            {
                int yaRecibido = recepcionesPrevias
                    .Where(dr => dr.VarianteId == dp.VarianteId)
                    .Sum(dr => dr.CantidadRecibida);

                return new ItemPedidoPendienteDTO
                {
                    PedidoId = dp.PedidoId,
                    VarianteId = dp.VarianteId,
                    NombreProducto = dp.Variante != null ? dp.Variante.NombreProducto : "Producto",
                    CantidadPedida = dp.Cant_prod_Pedido,
                    CantidadYaRecibida = yaRecibido,
                    PrecioUnitario = dp.Valor_uni,
                    CantidadARecibirAhora = 0
                };
            }).ToList();

            return itemsPendientes;
        }
    }
}
