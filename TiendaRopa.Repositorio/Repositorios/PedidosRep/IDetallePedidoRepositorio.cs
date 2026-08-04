using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos.Entity;

namespace TiendaRopa.Repositorio.Repositorios.PedidosRep
{
    public interface IDetallePedidoRepositorio
    {
        Task<IEnumerable<DetallesPedido>> GetAllAsync();
        Task<DetallesPedido?> GetByIdAsync(int id);
        Task AddAsync(DetallesPedido detalle);
        Task UpdateAsync(DetallesPedido detalle);
        Task DeleteAsync(int id);
    }
}
