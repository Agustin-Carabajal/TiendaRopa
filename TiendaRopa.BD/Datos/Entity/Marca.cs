using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Marca : EntityBase
    {
        [Required(ErrorMessage = "El nombre de la marca del producto es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre de la marca del producto no puede exceder los 100 caracteres.")]
        public string NombreMarca { get; set; } = string.Empty;
    }
}
