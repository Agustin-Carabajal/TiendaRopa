using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Proveedor : EntityBase
    {
        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El nombre del proveedor no puede exceder los 200 caracteres.")]
        public required string RazonSocialProveedores { get; set; }

        [MaxLength(50, ErrorMessage = "El cuit de proveedores no puede exceder los 50 caracteres.")]
        public required string CuitProveedores { get; set; }

        [MaxLength(200, ErrorMessage = "La direccion del domicilio no puede exceder los 200 caracteres.")]
        public required string DomicilioProveedores { get; set; }

        [MaxLength(50, ErrorMessage = "El nombre del contacto no puede exceder los 50 caracteres.")]
        public required string ContactoNombreProveedores { get; set; }

        [MaxLength(50, ErrorMessage = "El Correo Electronico del proveedor no puede exceder los 50 caracteres.")]
        public required string EmailProveedores { get; set; }
    }
}
