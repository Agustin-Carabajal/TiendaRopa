using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Pago : EntityBase
    {
        [Required(ErrorMessage = "El tipo de pago es obligatorio")]
        [MaxLength(50, ErrorMessage = "El tipo de pago no puede exceder los 50 caracteres.")]
        public string Tipo { get; set; } = string.Empty;
    }
}
