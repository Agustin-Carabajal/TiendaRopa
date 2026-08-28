using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Cliente
{
    public class ClienteCrearDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public required string Nombre { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres.")]
        public string? Apellido { get; set; }

        [MaxLength(8, ErrorMessage = "El DNI no puede exceder los 8 caracteres.")]
        public string? Dni { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [MaxLength(300, ErrorMessage = "El domicilio no puede exceder los 300 caracteres.")]
        public string? Domicilio { get; set; }

        [MaxLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        public string? Telefono { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }

        [MaxLength(50, ErrorMessage = "El rol no puede exceder los 50 caracteres.")]
        public string Rol { get; set; } = string.Empty;

     
    }
}
