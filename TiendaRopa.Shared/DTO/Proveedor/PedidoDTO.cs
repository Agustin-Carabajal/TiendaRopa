using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Proveedor
{
    public class PedidoDTO
    {
       
            public int Id { get; set; }
            public decimal TotalPedido { get; set; }
            public DateTime FechaDePedido { get; set; } = DateTime.Now;
            public DateTime? FechaDeEntrega { get; set; }
            public string FacturaPedido { get; set; } = string.Empty;
            public int IdProveedor { get; set; }
            public string? RazonSocialProveedor { get; set; }
    }
}
