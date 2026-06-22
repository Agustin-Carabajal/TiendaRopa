using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class TalleCrearDTO
    {
        [Required(ErrorMessage = "El nombre del talle es obligatorio.")]
        [MaxLength(10, ErrorMessage = "El nombre del talle no puede exceder los 100 caracteres.")]
        public string NombreTalle { get; set; } = string.Empty;
    }
}
