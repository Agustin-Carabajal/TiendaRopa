using System.ComponentModel.DataAnnotations;
namespace TiendaRopa.Shared.DTO.Venta;
public class PagoDTO { public int Id { get; set; } [Required,MaxLength(50)] public string Tipo { get; set; } = string.Empty; }
