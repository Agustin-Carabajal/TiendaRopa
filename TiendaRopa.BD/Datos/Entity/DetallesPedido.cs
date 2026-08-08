using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class DetallesPedido : EntityBase
    {
        public int Cant_prod_Pedido { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor_est { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor_uni { get; set; }

        // Foraneas

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
    }
}
