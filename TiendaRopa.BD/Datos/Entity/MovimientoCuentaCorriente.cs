using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.BD.Datos.Entity;

/// <summary>
/// Registra cada cargo o pago que modifica el Saldo de un Cliente (fiado),
/// para poder auditar por qué cambió ese saldo a lo largo del tiempo.
/// </summary>
public class MovimientoCuentaCorriente : EntityBase
{
    public DateTime Fecha { get; set; }
    public TipoMovimientoCuentaCorriente Tipo { get; set; }
    public decimal Monto { get; set; }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    // Nullable: un pago a cuenta puede no estar asociado a una venta puntual.
    public int? VentaId { get; set; }
    public Venta? Venta { get; set; }
}
