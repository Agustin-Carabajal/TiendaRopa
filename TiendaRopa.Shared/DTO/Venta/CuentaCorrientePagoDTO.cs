using System.ComponentModel.DataAnnotations;
namespace TiendaRopa.Shared.DTO.Venta;
public class CuentaCorrientePagoDTO { [Range(0.01,999999999)] public decimal Monto { get; set; } }
