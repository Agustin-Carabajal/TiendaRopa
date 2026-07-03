using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class ProductoColorMostrarDTO
    {
        public string UrlImagen { get; set; } = string.Empty; 

        public string ProductoNombre { get; set; } = string.Empty; //Where ProductoId = Producto.Id => ProductoNombre = Producto.Nombre

        public string ColorNombre { get; set; } = string.Empty; //Where ColorId = Color.Id => ColorNombre = Color.NombreColor

        public EstadoRegistro Estado { get; set; } = EstadoRegistro.activo;
    }
    
}
