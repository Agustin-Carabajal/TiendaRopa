using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Proveedor
{
    public class DetallesRecepcionDto
    {
        public int Id { get; set; }
        public decimal TotalPedidos { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int CantidadRecibida { get; set; }
        public int PedidoId { get; set; }
        public int RecepcionId { get; set; }
        public int VarianteId { get; set; }
        public string? NombreProducto { get; set; }
    }
}
