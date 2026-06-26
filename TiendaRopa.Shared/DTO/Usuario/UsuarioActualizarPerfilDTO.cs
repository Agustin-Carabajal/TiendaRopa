using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Usuario
{
    public class UsuarioActualizarPerfilDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; } = string.Empty;
    }
}
