namespace TiendaRopa.Shared.DTO.Venta;
public class CuentaCorrienteMovimientoDTO { public int ClienteId { get; set; } public decimal Saldo { get; set; } public DateTime Fecha { get; set; } public string Tipo { get; set; } = string.Empty; public decimal Monto { get; set; } public int? VentaId { get; set; } }
