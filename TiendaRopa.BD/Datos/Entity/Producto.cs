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




  
       

        //Foraneas



        public int? ProveedorId { get; set; }
        public Proveedor? Proveedor  { get; set; }

        public int MarcaId { get; set; }
        public Marca? Marca { get; set; }

    }
}
