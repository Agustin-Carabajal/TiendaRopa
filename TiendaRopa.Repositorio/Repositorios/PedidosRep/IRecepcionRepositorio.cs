using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Shared.DTO.Proveedor;

namespace TiendaRopa.Repositorio.Repositorios.PedidosRep
{
    public interface IRecepcionRepositorio
    {
        Task<IEnumerable<Recepcion>> GetAllAsync();
        Task<Recepcion?> GetByIdAsync(int id);
        Task<Recepcion> AddRecepcionAsync(Recepcion recepcion); // <-- Firma para la cabecera
        Task AddDetalleRecepcionAsync(DetallesRecepcion detalle);
        Task<IEnumerable<ItemPedidoPendienteDTO>> GetItemsPendientesPorPedidoAsync(int pedidoId);
    }
}
