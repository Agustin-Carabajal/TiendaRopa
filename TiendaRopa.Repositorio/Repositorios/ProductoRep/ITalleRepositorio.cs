using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Repositorio.Repositorios.ProductoRep
{
    public interface ITalleRepositorio : IRepositorio<Talle>
    {
        Task<bool> ExisteNombre(string nombre);
        Task<List<TalleMostrarDTO>> SelectListaTalles();
    }
}