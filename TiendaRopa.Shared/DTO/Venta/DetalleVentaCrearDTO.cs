using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TiendaRopa.Shared.DTO.Venta
{
    public class DetalleVentaCrearDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un artículo.")]
        public int VarianteId { get; set; }

        [Range(1, 100000, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; } = 1;

        /// <summary>
        /// Precio al que se vende esta unidad. Si viene null el servidor usa
        /// Variante.PrecioVenta (alta) o conserva el precio ya guardado (edición).
        /// </summary>
        [Range(0.0, 99999999.99, ErrorMessage = "El precio unitario no es válido.")]
        public decimal? PrecioUnitario { get; set; }
    }
}
