using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos.Entity;

namespace TiendaRopa.Repositorio.Repositorios.PedidosRep
{
    public interface IPedidoRepositorio
    {
        Task<IEnumerable<Pedido>> GetAllAsync();
        Task<Pedido?> GetByIdAsync(int id);
        Task AddAsync(Pedido pedido);
        Task UpdateAsync(Pedido pedido);
        Task DeleteAsync(int id);
    }
}
