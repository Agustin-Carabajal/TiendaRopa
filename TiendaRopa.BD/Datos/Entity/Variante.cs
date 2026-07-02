using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Variante : EntityBase
    {
        [MaxLength(100, ErrorMessage = "El código no puede exceder los 100 caracteres.")]
        public string CodVariante { get; set; } = string.Empty;

        [Required(ErrorMessage = "El stock es obligatorio.")]
        public required int Stock { get; set; }

        [Required(ErrorMessage = "El precio de venta es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        public required decimal PrecioVenta { get; set; }

        public bool Activo { get; set; } = true;

        //Foraneas

        public int ProductoColorId { get; set; }
        public ProductoColor? ProductoColor { get; set; }

        public int TalleId { get; set; }
        public Talle? Talle { get; set; }
    }
}
