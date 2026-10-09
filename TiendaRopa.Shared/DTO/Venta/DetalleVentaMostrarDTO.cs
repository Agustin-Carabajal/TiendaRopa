using System;
using System.Collections.Generic;

namespace TiendaRopa.Shared.DTO.Venta
{
    public class DetalleVentaMostrarDTO
    {
        public int Id { get; set; }
        public int VarianteId { get; set; }
        public string CodVariante { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Talle { get; set; } = string.Empty;
        public string? UrlImagen { get; set; }

        public int Cantidad { get; set; }

        /// <summary>Precio al que se vendió esa unidad (histórico, no el precio actual de la variante).</summary>
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        /// <summary>Stock actual de la variante. La pantalla de edición lo usa para calcular el máximo permitido.</summary>
        public int StockActual { get; set; }
    }
}
