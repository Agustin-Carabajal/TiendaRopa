using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.Shared.DTO.Proveedor
{
    public class ItemRecepcionDTO
    {
        int VarianteId { get; set; }
        string NombreProducto { get; set; } = string.Empty;
        int CantidadPedida { get; set; }
        int CantidadYaRecibida { get; set; } // Suma de DetallesRecepcion anteriores
        int CantidadPendiente => CantidadPedida - CantidadYaRecibida;

        // Lo que el usuario va a tipear en el formulario de la nueva recepción:
        public int CantidadARecibirHoy { get; set; }
    }
}
