using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.ApplicationUser
{
    public class UsuarioActualizarAdminDTO
    {
        public int IntId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string ApellidoUsuario { get; set; } = string.Empty;
        public string DniUsuario { get; set; } = string.Empty;
        public DateTime FechaNacimientoUsuario { get; set; }
        public string DireccionUsuario { get; set; } = string.Empty;

        // Campos exclusivos que controla el dueño de la tienda
        public decimal SaldoUsuario { get; set; }
        public EstadoRegistro EstadoRegistro { get; set; }
        public string Observacion { get; set; } = "";
    }
}
