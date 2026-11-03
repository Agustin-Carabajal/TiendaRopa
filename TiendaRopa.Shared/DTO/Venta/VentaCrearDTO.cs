using System.ComponentModel.DataAnnotations;
namespace TiendaRopa.Shared.DTO.Venta;
public class VentaCrearDTO { [Range(1,int.MaxValue,ErrorMessage="Debe seleccionar un cliente.")] public int ClienteId { get; set; } public int? PagoId { get; set; } public List<PagoVentaCrearDTO> Pagos { get; set; } = new(); public decimal MontoCuentaCorriente { get; set; } [Required,MinLength(1,ErrorMessage="La venta debe tener al menos un artículo.")] public List<DetalleVentaCrearDTO> Detalles { get; set; } = new(); }
