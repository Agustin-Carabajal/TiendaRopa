using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Producto : EntityBase
    {
        //[MaxLength(500, ErrorMessage = "La ruta de la imagen no puede exceder los 500 caracteres.")]
        //public string? ImagenUrl { get; set; }

        [MaxLength(30, ErrorMessage = "El código del producto no puede exceder los 30 caracteres.")]
        public string CodProducto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El nombre del producto no puede exceder los 200 caracteres.")]
        public required string NombreProducto { get; set; }

        [Required(ErrorMessage = "El stock del producto es obligatorio.")]
        public required int StockProducto { get; set; }

        public decimal PrecioVentaProducto { get; set; }

        public decimal PrecioCompraProducto { get; set; }

        //Foraneas

        [Required(ErrorMessage = "La marca del producto es obligatoria.")]
        public required int MarcaId { get; set; }
        public Marca? Marca { get; set; } 

        [Required(ErrorMessage = "El tamaño del producto es obligatorio.")]
        public required int TalleId { get; set; }
        public Talle? Talle { get; set; }

        [Required(ErrorMessage = "El color del producto es obligatorio.")]
        public required int ColorId { get; set; }
        public Color? Color { get; set; }

        [Required(ErrorMessage = "El proveedor del producto es obligatorio.")]
        public required int ProveedorId { get; set; }
        public Proveedor? Proveedor  { get; set; }

    }
}
