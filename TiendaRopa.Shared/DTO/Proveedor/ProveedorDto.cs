using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.Shared.DTO.Proveedor
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [StringLength(200, ErrorMessage = "La razón social no puede exceder los 200 caracteres.")]
        public string RazonSocialProveedores { get; set; } = string.Empty;
        public string? CuitProveedores { get; set; }
        public string? DomicilioProveedores { get; set; }
        public string? ContactoNombreProveedores { get; set; }
        [Required(ErrorMessage = "El número de contacto es obligatorio.")]
        [StringLength(30, ErrorMessage = "El número de contacto no puede exceder los 30 caracteres.")]
        public string TelefonoContacto { get; set; } = string.Empty;
        public string? EmailProveedores { get; set; }
        public string? ObvsProveedores { get; set; }
    }
}
