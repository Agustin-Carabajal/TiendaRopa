using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Usuario
{
    public class UsuarioMostrarDTO
    {
        public string? Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
    }
}
