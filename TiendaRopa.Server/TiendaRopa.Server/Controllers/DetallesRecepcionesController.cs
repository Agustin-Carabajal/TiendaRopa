using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.PedidosRep;
using TiendaRopa.Shared.DTO.Proveedor;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetallesRecepcionesController : ControllerBase
    {
        private readonly IRecepcionRepositorio _repository;

        public DetallesRecepcionesController(IRecepcionRepositorio repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DetallesRecepcionDto dto)
        {
            var detalle = new DetallesRecepcion
            {
                PedidoId = dto.PedidoId,
                RecepcionId = dto.RecepcionId,
                VarianteId = dto.VarianteId,
                CantidadRecibida = dto.CantidadRecibida,
                PrecioUnitario = dto.PrecioUnitario,
                TotalPedidos = dto.TotalPedidos
            };

            await _repository.AddDetalleRecepcionAsync(detalle);
            return Ok();
        }
    }
}

