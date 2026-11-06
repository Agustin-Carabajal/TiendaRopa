using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Repositorio.Repositorios.ProductoRep
{
    public interface IEtiquetaRepositorio : IRepositorio<Etiqueta>
    {
        Task<bool> ExisteNombre(string nombre);
        Task<List<EtiquetaMostrarDTO>> SelectListaEtiquetas();
    }
}
