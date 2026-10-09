using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Cliente;
using TiendaRopa.Shared.DTO.Producto_y_mas;
using TiendaRopa.Shared.DTO.Venta;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Repositorio.Repositorios.VentasRep
{
    public interface IVentaRepositorio : IRepositorio<Venta>
    {
        // Consultas
        Task<List<VentaMostrarDTO>> GetHistorial(CanalVenta? canal = null, DateTime? desde = null, DateTime? hasta = null, int? clienteId = null);
        Task<VentaDetalleDTO?> ObtenerById(int id);

        // Comandos. Crear recibe canal y carrito para que la compra web reutilice el mismo método.
        Task<ResultadoVenta> Crear(VentaCrearDTO dto, CanalVenta canal, int? carritoId = null);
        Task<ResultadoVenta> Editar(int id, VentaCrearDTO dto);
        Task<ResultadoVenta> Anular(int id);

        // Datos para armar la pantalla de venta
        Task<List<ClienteMostrarDTO>> GetClientesActivos();
        Task<List<VarianteMostrarDTO>> GetVariantesDisponibles();
        Task<List<PagoMostrarDTO>> GetMetodosPago();
    }
}
