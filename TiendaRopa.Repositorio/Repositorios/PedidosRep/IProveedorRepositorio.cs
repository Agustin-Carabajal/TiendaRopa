using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos.Entity;

namespace TiendaRopa.Repositorio.Repositorios.PedidosRep
{
    public interface IProveedorRepositorio
    {
        Task<IEnumerable<Proveedor>> GetAllAsync();
        Task<Proveedor?> GetByIdAsync(int id);
        Task AddAsync(Proveedor proveedor);
        Task UpdateAsync(Proveedor proveedor);
        Task DeleteAsync(int id);
    }
}
