using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Producto : EntityBase
    {

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre del producto no puede exceder los 100 caracteres.")]
        public required string NombreProducto { get; set; }

        public string? DescripcionProducto { get; set; }

        [Required(ErrorMessage = "El nombre de la marca del producto es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre de la marca del producto no puede exceder los 100 caracteres.")]
        public required string MarcaProducto { get; set; }


  
       

        //Foraneas


        [Required(ErrorMessage = "El proveedor del producto es obligatorio.")]
        public required int ProveedorId { get; set; }
        public Proveedor? Proveedor  { get; set; }

    }
}
