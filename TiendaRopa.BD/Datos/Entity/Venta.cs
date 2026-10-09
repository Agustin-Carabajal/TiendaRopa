using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.BD.Datos.Entity;

public class Venta : EntityBase
{
    public DateTime FechaHora { get; set; }
    public CanalVenta CanalDeVenta { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal Monto { get; set; }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    // Nullable: las ventas presenciales se cargan directo, sin pasar por un carrito.
    public int? CarritoId { get; set; }
    public Carrito? Carrito { get; set; }

    public Envio? Envio { get; set; }
    public ICollection<PagoVenta> PagosVenta { get; set; } = new List<PagoVenta>();
    public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
}
