using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Talle : EntityBase
    {
        [Required(ErrorMessage = "El nombre del talle es obligatorio.")]
        public required string NombreTalle { get; set; }
    }
}
