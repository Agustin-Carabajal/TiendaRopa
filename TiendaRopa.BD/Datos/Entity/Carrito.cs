using TiendaRopa.BD.Datos;

namespace TiendaRopa.BD.Datos.Entity;

public class Carrito : EntityBase
{
    public DateTime FechaCreacion { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal Monto { get; set; }

    public int CompradorId { get; set; }
    public Comprador Comprador { get; set; } = null!;

    // Relación 1 a 1: un carrito confirmado genera a lo sumo una venta.
    public Venta? Venta { get; set; }
}