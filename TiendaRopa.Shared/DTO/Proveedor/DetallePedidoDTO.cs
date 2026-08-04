using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Proveedor
{
    public class DetallePedidoDTO
    {
        public int Id { get; set; }
        public int Cant_prod_Pedido { get; set; }
        public decimal Valor_est { get; set; }
        public decimal Valor_uni { get; set; }

        public int PedidoId { get; set; }
        public string? FacturaPedido { get; set; } 

        public int ProductoId { get; set; }
        public string? NombreProducto { get; set; }
    }
}
