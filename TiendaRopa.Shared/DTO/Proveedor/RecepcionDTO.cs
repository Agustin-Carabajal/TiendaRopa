using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Proveedor
{
    public class RecepcionDto
    {
        public int Id { get; set; }
        public DateTime FechaDeEntrega { get; set; } = DateTime.Now;
        public decimal PrecioEnvioTotal { get; set; }
        public string RemitoProveedor { get; set; } = string.Empty;
        public string RecibidoPor { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;
        public int? PedidoId { get; set; }
        public string? FacturaPedido { get; set; }
    }
}
