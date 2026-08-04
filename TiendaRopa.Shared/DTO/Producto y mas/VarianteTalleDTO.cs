using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class VarianteTalleDTO
    {
        public int TalleId { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El precio de venta es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal PrecioVenta { get; set; }

        [MaxLength(100, ErrorMessage = "El código no puede exceder los 100 caracteres.")]
        public string CodVariante { get; set; } = string.Empty;

      
    }
}
