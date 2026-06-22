using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Color : EntityBase
    {
        [Required(ErrorMessage = "El nombre del color es obligatorio.")]
        public required string NombreColor { get; set; }
    }
}
