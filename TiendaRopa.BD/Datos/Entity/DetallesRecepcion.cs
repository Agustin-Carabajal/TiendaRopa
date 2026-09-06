using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class DetallesRecepcion : EntityBase
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPedidos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        public int CantidadRecibida { get; set; }


        //Foraneas

        [Required(ErrorMessage = "")]
        public required int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }

        [Required(ErrorMessage = "")]
        public required int RecepcionId { get; set; }
        public Recepcion? Recepcion { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio.")]
        public required int VarianteId { get; set; }
        public Producto? Variante { get; set; }
    }
}
