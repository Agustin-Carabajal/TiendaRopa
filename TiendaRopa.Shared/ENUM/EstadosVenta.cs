namespace TiendaRopa.Shared.ENUM
{
    /// <summary>
    /// Valores posibles de Venta.Estado (la entidad lo guarda como string).
    /// Al integrar la tienda web se pueden sumar acá estados como "Pendiente" o "Enviada".
    /// </summary>
    public static class EstadosVenta
    {
        public const string Completada = "Completada";
        public const string Anulada = "Anulada";
    }
}
