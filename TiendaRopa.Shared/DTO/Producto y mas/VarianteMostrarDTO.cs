using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class VarianteMostrarDTO
    {
        public int Id { get; set; }
        public string CodVariante { get; set; } = string.Empty;
        public int ProductoId { get; set; }
        public string ProductoColor { get; set; } = " - ";
        public string Talle { get; set; } = string.Empty;
        public  int Stock { get; set; }
  
        [Column(TypeName = "decimal(18,2)")]
        public  decimal PrecioVenta { get; set; }
        public EstadoRegistro Estado { get; set; }
    }
}
