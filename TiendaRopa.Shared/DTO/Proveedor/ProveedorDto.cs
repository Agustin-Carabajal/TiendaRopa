using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Proveedor
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        public string RazonSocialProveedores { get; set; } = string.Empty;
        public string CuitProveedores { get; set; } = string.Empty;
        public string DomicilioProveedores { get; set; } = string.Empty;
        public string ContactoNombreProveedores { get; set; } = string.Empty;
        public string EmailProveedores { get; set; } = string.Empty;
        public string? ObvsProveedores { get; set; }
    }
}
