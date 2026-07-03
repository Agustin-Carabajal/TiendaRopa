using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Repositorio.Repositorios.ProductoRep
{
    public interface IProductoRepositorio : IRepositorio<Producto>

    {
        Task<bool> Editar(int id, ProductoCrearDTO dto);
        Task<List<ProductoMostrarDTO>> GetListaActivos();
        Task<List<ProductoMostrarDTO>> GetListaInactivos();
        Task<ProductoMostrarDTO?> ObtenerById(int id);
    }
}