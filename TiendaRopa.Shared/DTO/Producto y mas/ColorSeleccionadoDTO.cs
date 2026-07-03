using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class ColorSeleccionadoDTO
    {
        [Required]
        public int ColorId { get; set; }

        // Cada color seleccionado puede tener una imagen única cargada por el usuario
        public string UrlImagen { get; set; } = string.Empty;

        // ==========================================
        // 3. TALLES PARA ESTE COLOR (CHECKBOXES NIVEL 2)
        // ==========================================
        // Cada color tendrá su propio desglose de talles, stocks y precios
        public List<VarianteTalleDTO> Variantes { get; set; } = new();
    }
}
