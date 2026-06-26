using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Usuario
{
    public class UsuarioActualizarAdminDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; } = string.Empty;

        // Campos exclusivos que controla el dueño de la tienda
        public decimal Saldo { get; set; }
        public EstadoRegistro EstadoRegistro { get; set; }
        public string Observacion { get; set; } = "";
    }
}
