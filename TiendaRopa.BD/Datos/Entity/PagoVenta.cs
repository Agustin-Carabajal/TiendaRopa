using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class PagoVenta : EntityBase
    {
        public int PagoId { get; set; }
        public Pago? Pago { get; set; }

        public int VentaId { get; set; }
        public Venta? Venta { get; set; }
    }
}
