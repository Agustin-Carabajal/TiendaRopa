using System;
using System.Collections.Generic;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Venta
{
    /// <summary>Venta completa con cada artículo vendido y su precio individual.</summary>
    public class VentaDetalleDTO
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public CanalVenta Canal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal Monto { get; set; }

        public int ClienteId { get; set; }
        public string Cliente { get; set; } = string.Empty;

        public int? PagoId { get; set; }
        public string? MetodoPago { get; set; }

        public List<DetalleVentaMostrarDTO> Detalles { get; set; } = new();
    }
}
