using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Proveedor
{
    public class DetallePedidoDTO
    {
        public int Id { get; set; }
        public int Cant_prod_Pedido { get; set; }
        public int PedidoId { get; set; }
        public string? FacturaPedido { get; set; } 

        public int VarianteId { get; set; }
        public string? NombreProducto { get; set; }
        public string? CodVariante { get; set; }
        public string? Color { get; set; }
        public string? Talle { get; set; }
    }
}
