using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Repositorio.Repositorios.Producto
{
    public interface IMarcaRepositorio
    {
        Task<List<MarcaMostrarDTO>> SelectListaMarcas();
    }
}