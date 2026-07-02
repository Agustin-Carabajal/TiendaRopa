using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class ProductoColor : EntityBase
    {
        public string UrlImagen { get; set; } = string.Empty;


        //Foraneas
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int ColorId { get; set; }
        public Color? Color { get; set; }
    }
}
