using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class DetallesPedido : EntityBase
    {
        public int Cant_prod_Pedido { get; set; }
        public decimal Valor_est { get; set; }
        public decimal Valor_uni { get; set; }

        // Foraneas

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
    }
}
