using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TiendaRopa.Shared.ENUM;
using System.ComponentModel.DataAnnotations;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class ProductoCrearDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una marca.")]
        public int MarcaId { get; set; }
        public int? ProveedorId { get; set; }
        public EstadoRegistro Estado { get; set; }
    }
}
