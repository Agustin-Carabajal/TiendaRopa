using System;
using System.Collections.Generic;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Venta
{
    /// <summary>Fila del historial de ventas.</summary>
    public class VentaMostrarDTO
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public CanalVenta Canal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal Monto { get; set; }

        public int ClienteId { get; set; }
        public string Cliente { get; set; } = string.Empty;

        public string? MetodoPago { get; set; }
        public int CantidadArticulos { get; set; }
    }
}
