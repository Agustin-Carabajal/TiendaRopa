using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class ProductoMostrarDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public string Marca { get; set; } = string.Empty;

        public string Proveedor {  get; set; } = string.Empty;

        public EstadoRegistro Estado { get; set; }
    }
}
