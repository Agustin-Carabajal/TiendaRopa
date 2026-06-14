using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.BD.Datos
{
    public class EntityBase : IEntityBase
    {
        public int Id { get; set; }
        public EstadoRegistro EstadoRegistro { get; set; }
        public string Observacion { get; set; } = "";
    }
}
