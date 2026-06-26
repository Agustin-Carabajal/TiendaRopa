using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TiendaRopa.Shared.DTO.Usuario
{
    public class UsuarioCrearDTO
    {
        // 1. Credenciales de acceso críticas para Identity
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;

        // 2. Información personal que definiste en tu tabla
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        public string Dni { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }

        public string Direccion { get; set; } = string.Empty;

        // 3. Datos operativos de la tienda
        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }

        // Tip Pro: Puedes agregar el rol que tendrá el usuario al crearse
        public string Rol { get; set; } = "Cliente";
    }
}
