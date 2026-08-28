using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Shared.DTO.Cliente
{
    public class ClienteMostrarDTO
    {
        public int Id { get; set; }
        public required string Nombre { get; set; } = string.Empty;

        public string? Apellido { get; set; }

        public string? Dni { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string? Domicilio { get; set; }

        public string? Telefono { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }

        public string Rol { get; set; } = string.Empty;
    }
}
