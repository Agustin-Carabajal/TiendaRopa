using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.BD.Datos
{
    public class ApplicationUser : IdentityUser
    {
        public EstadoRegistro EstadoRegistro { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
