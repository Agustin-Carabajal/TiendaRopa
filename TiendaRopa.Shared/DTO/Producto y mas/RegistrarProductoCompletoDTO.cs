using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class RegistrarProductoCompletoDTO
    {
        // ==========================================
        // 1. DATOS BÁSICOS DEL PRODUCTO
        // ==========================================
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string NombreProducto { get; set; } = string.Empty;

        public string? DescripcionProducto { get; set; }

        [Required(ErrorMessage = "La marca del producto es obligatoria.")]
        [MaxLength(100, ErrorMessage = "La marca no puede exceder los 100 caracteres.")]
        public string MarcaProducto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        public int ProveedorId { get; set; }

        public bool Activo { get; set; } = true;


     

        // ==========================================
        // 2. COLORES SELECCIONADOS (CHECKBOXES NIVEL 1)
        // ==========================================
        // Aquí se guardarán solo los colores que el usuario marque con el check
        public List<ColorSeleccionadoDTO> Colores { get; set; } = new();
    }
}
