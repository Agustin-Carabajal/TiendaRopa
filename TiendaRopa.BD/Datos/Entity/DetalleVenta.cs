

namespace TiendaRopa.BD.Datos.Entity;

/// <summary>
/// Línea de detalle de una Venta, análoga a DetallePedido/DetalleRecepcion.
/// PrecioUnitario se guarda (no se recalcula desde Variante.PrecioVenta) porque
/// el precio puede cambiar después de la venta y esta fila es el registro
/// histórico de a cuánto se vendió realmente ese producto ese día.
/// </summary>
public class DetalleVenta : EntityBase
{
    public int CantidadProducto { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }

    public int VentaId { get; set; }
    public Venta Venta { get; set; } = null!;

    public int VarianteId { get; set; }
    public Variante Variante { get; set; } = null!;
}