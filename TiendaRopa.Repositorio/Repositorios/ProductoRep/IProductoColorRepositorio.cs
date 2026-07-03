using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Repositorio.Repositorios.ProductoRep
{
    public interface IProductoColorRepositorio : IRepositorio<ProductoColor>
    {
        Task<bool> Editar(int id, ProductoColorEditarDTO dto);
        Task<List<ProductoColorMostrarDTO>> GetListaActivos();
        Task<List<ProductoColorMostrarDTO>> GetListaInactivos();
        Task<ProductoColorMostrarDTO?> ObtenerById(int id);
    }
}