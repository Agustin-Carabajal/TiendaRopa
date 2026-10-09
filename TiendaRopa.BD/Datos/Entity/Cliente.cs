using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Cliente : EntityBase
    {
        public required string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Dni { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public string? Domicilio { get; set; }
        public string? Telefono { get; set; }

        public decimal Saldo { get; set; }
        public OrigenCliente Origen { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
    }
}
