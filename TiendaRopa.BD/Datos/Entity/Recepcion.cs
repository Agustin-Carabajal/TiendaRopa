using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Recepcion : EntityBase
    {
        public DateTime FechaDePedido { get; set; }

        [MaxLength(45, ErrorMessage = "El Remito del proveedor no puede exceder los 45 caracteres.")]
        public required string RemitoProveedor { get; set; }

        [MaxLength(45, ErrorMessage = "EL nombre del receptor no puede exceder los 45 caracteres.")]
        public required string RecibidoPor { get; set; }

        [MaxLength(255, ErrorMessage = "Las notas no pueden exceder los 255 caracteres.")]
        public required string Notas { get; set; }
    }
}
