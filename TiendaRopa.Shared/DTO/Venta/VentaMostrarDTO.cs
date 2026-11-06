using TiendaRopa.Shared.ENUM;
namespace TiendaRopa.Shared.DTO.Venta;
public class VentaMostrarDTO { public int Id { get; set; } public DateTime FechaHora { get; set; } public CanalVenta Canal { get; set; } public string Estado { get; set; }=string.Empty; public decimal Monto { get; set; } public int ClienteId { get; set; } public string Cliente { get; set; }=string.Empty; public List<PagoVentaMostrarDTO> Pagos { get; set; }=new(); public decimal MontoCuentaCorriente { get; set; } public int CantidadArticulos { get; set; } }
