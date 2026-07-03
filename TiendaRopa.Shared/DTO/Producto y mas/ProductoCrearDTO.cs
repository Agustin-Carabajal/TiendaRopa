using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class ProductoCrearDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public string Marca { get; set; } = string.Empty;
        public int ProveedorId { get; set; }
        public EstadoRegistro Estado { get; set; }
    }
}
