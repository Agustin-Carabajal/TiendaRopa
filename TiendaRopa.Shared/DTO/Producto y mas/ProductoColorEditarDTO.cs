using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class ProductoColorEditarDTO
    {
        public string UrlImagen { get; set; } = string.Empty;
        public int ProductoId { get; set; }
        public int ColorId { get; set; }
        public EstadoRegistro Estado { get; set; }
    }
}
