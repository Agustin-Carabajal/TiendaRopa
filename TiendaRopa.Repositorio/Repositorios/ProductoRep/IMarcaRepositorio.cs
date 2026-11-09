using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Repositorio.Repositorios.ProductoRep
{
    public interface IMarcaRepositorio : IRepositorio<Marca>
    {
        Task<List<MarcaMostrarDTO>> SelectListaMarcas();
        Task<bool> ExisteNombre(string nombre);
    }
}
