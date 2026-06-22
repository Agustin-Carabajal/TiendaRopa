using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO
{
    public class UsuarioActualizarPerfilDTO
    {
        public int IntId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string ApellidoUsuario { get; set; } = string.Empty;
        public string DniUsuario { get; set; } = string.Empty;
        public DateTime FechaNacimientoUsuario { get; set; }
        public string DireccionUsuario { get; set; } = string.Empty;
    }
}
