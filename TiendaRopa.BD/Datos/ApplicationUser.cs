using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.BD.Datos
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty; // Ej: "Juan" (UserName se usa para el login)

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Dni { get; set; } = string.Empty;

        public DateTime FechaNacimiento { get; set; }

        [MaxLength(250)]
        public string Direccion { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }

        [MaxLength(300)]
        public string Observacion { get; set; } = string.Empty;
        public EstadoRegistro EstadoRegistro { get; set; }
    }
}
