using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class ColorCrearDTO
    {
        [Required(ErrorMessage = "El nombre del color es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El nombre del color no puede exceder los 200 caracteres.")]
        public string NombreColor { get; set; } = string.Empty;
    }
}
