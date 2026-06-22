using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Pedido : EntityBase
    {
        public decimal TotalPedidos { get; set; }

        public DateTime FechaDePedido { get; set; }

        public DateTime FechaDeEntrega { get; set; }

        [MaxLength(255, ErrorMessage = "La Factura del pedido no puede exceder los 255 caracteres.")]
        public required string FacturaPedidos { get; set; }
    }
}
