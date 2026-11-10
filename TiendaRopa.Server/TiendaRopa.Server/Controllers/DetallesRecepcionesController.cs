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
            if (dto.CantidadRecibida <= 0)
                return BadRequest("La cantidad recibida debe ser mayor a cero.");
            if (dto.PrecioUnitario <= 0 || dto.PrecioEnvio < 0 || dto.Ganancia < 0)
                return BadRequest("El costo unitario debe ser mayor a cero y el envío y la ganancia no pueden ser negativos.");

            var detalle = new DetallesRecepcion
            {
                PedidoId = dto.PedidoId,
                RecepcionId = dto.RecepcionId,
                VarianteId = dto.VarianteId,
                CantidadRecibida = dto.CantidadRecibida,
                PrecioUnitario = dto.PrecioUnitario,
                PrecioEnvio = dto.PrecioEnvio,
                Ganancia = dto.PrecioUnitario * 2m,
                TotalPedidos = dto.TotalPedidos
            };

            await _repository.AddDetalleRecepcionAsync(detalle);
            return Ok();
        }
    }
}

