namespace TiendaRopa.Repositorio.Repositorios.VentasRep
{
    /// <summary>
    /// Resultado de una operación de venta. Permite devolver el motivo exacto del rechazo
    /// (stock insuficiente, cliente inexistente, etc.) para mostrarlo en pantalla.
    /// </summary>
    public record ResultadoVenta(bool Exito, string? Error = null, int VentaId = 0, bool NoEncontrada = false)
    {
        public static ResultadoVenta Ok(int ventaId) => new(true, null, ventaId);
        public static ResultadoVenta Falla(string error) => new(false, error);
        public static ResultadoVenta NoExiste(int id) => new(false, $"No existe la venta con ID: {id}.", id, true);
    }
}
