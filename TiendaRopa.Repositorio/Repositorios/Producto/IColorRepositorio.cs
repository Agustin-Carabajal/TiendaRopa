using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Repositorio.Repositorios.Producto
{
    public interface IColorRepositorio : IRepositorio<Color>
    {
        Task<bool> ExisteNombre(string nombre);
        Task<List<ColorMostrarDTO>> SelectListaColores();
    }
}