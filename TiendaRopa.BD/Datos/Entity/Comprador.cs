using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.BD.Datos.Entity;

/// <summary>
/// Tabla puente entre Clientes (ventas presenciales) y ApplicationUser (ventas web).
/// Exactamente uno de los dos FK debe estar cargado (se garantiza con un CHECK
/// constraint a nivel de base de datos, ver CompradorConfiguration).
/// </summary>
public class Comprador : EntityBase
{
    public int? ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    // OJO: ApplicationUser (IdentityUser) usa clave primaria de tipo string por defecto.
    // Si en tu proyecto la personalizaste a Guid, cambiá este tipo para que coincida.
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

    public OrigenComprador Origen { get; set; }

    public ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}

