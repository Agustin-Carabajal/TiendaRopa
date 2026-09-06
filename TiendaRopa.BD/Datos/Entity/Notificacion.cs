using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.BD.Datos.Entity;

/// <summary>
/// Guarda el estado de una notificación (ya mostrada, leída, etc). El dato que
/// dispara la notificación (fecha de nacimiento, fecha de entrega) ya vive en
/// otras tablas; esta tabla evita repetir el mismo aviso una vez generado.
/// </summary>
public class Notificacion : EntityBase
{
    public TipoNotificacion Tipo { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public DateTime FechaGeneracion { get; set; } = DateTime.UtcNow;
    public bool Leida { get; set; } = false;

    // Referencia polimórfica: apunta a Cliente, ApplicationUser o Pedido según TipoEntidad,
    // sin necesitar una FK real (que obligaría a tres columnas nullable distintas).
    // string y no int: esta referencia es polimórfica y puede apuntar tanto a
    // entidades con Id int (Cliente, Pedido) como a ApplicationUser, cuyo Id es
    // string (default de Identity). Para Cliente/Pedido, guardar el int como texto.
    public string EntidadRelacionadaId { get; set; } = string.Empty;
    public TipoEntidadNotificacion TipoEntidad { get; set; }
}