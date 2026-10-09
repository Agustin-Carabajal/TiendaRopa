using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TiendaRopa.Shared.DTO.Venta
{
    /// <summary>
    /// Se usa tanto para crear como para editar una venta (mismo criterio que ProductoCrearDTO).
    /// El canal (Presencial/Web), la fecha y el estado los define el servidor, no el cliente.
    /// </summary>
    public class VentaCrearDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente.")]
        public int ClienteId { get; set; }

        /// <summary>Método de pago (tabla Pago). Opcional.</summary>
        public int? PagoId { get; set; }

        [Required(ErrorMessage = "La venta debe tener al menos un artículo.")]
        [MinLength(1, ErrorMessage = "La venta debe tener al menos un artículo.")]
        public List<DetalleVentaCrearDTO> Detalles { get; set; } = new();
    }
}
