using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Repositorio.Repositorios.Producto
{
    public interface ITalleRepositorio
    {
        Task<List<TalleMostrarDTO>> SelectListaTalles();
    }
}