using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Usuario
{
    public class UsuarioMostrarDTO
    {
        public string? Id { get; set; } = string.Empty;
        public int IntId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string ApellidoUsuario { get; set; } = string.Empty;
        public string DniUsuario { get; set; } = string.Empty;
        public decimal SaldoUsuario { get; set; }
    }
}
