using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Proveedor
{
    public class PedidoCreacionDTO
    {
        public int ProveedorId { get; set; }
        public DateTime FechaDePedido { get; set; }
        public DateTime? FechaDeEntrega { get; set; }
        public string FacturaPedido { get; set; }

        // Lista de ítems seleccionados en la UI
        public List<DetallePedidoDTO> Detalles { get; set; } = new();
    }
}
