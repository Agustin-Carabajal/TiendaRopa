using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Repositorio.Repositorios.ProductoRep
{
    public interface IVarianteRepositorio : IRepositorio<Variante>
    {
        Task<bool> Editar(int id, VarianteCrearDTO dto);
        Task<List<VarianteMostrarDTO>> GetListaActivos();
        Task<List<VarianteMostrarDTO>> GetListaInactivos();
        Task<VarianteMostrarDTO?> ObtenerById(int id);
    }
}