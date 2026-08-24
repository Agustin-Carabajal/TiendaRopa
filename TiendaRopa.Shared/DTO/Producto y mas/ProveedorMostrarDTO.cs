using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class ProveedorMostrarDTO
    {
        [JsonPropertyName("id")]
        public int IdProveedor { get; set; }
        public string RazonSocialProveedores { get; set; } = string.Empty;
    }
}
