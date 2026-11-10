using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Proveedor
{
    public class ItemPedidoPendienteDTO
    {
        public int PedidoId { get; set; }
        public int VarianteId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int CantidadPedida { get; set; }
        public int CantidadYaRecibida { get; set; }
        public int CantidadPendiente => CantidadPedida - CantidadYaRecibida;
        public decimal PrecioUnitario { get; set; }
        public decimal Ganancia => PrecioUnitario * 2m;
        public int CantidadARecibirAhora { get; set; }
    }
}
