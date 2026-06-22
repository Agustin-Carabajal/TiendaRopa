using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class MarcaCrearDTO
    {
        [Required(ErrorMessage = "El nombre de la marca es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El nombre de la marca no puede exceder los 200 caracteres.")]
        public string NombreMarca { get; set; } = string.Empty;
    }
}
