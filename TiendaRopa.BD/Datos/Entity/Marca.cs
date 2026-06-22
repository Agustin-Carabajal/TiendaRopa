using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Marca : EntityBase
    {
        [Required(ErrorMessage = "El nombre de la marca es obligatorio.")]
        public required string NombreMarca { get; set; }
    }
}
