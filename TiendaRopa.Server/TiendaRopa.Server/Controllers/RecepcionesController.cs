using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.PedidosRep;
using TiendaRopa.Shared.DTO.Proveedor;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecepcionesController : ControllerBase
    {
        private readonly IRecepcionRepositorio _repository;
        private readonly AppDbContext _context;

        public RecepcionesController(AppDbContext context, IRecepcionRepositorio repository)
        {
            _context = context;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecepcionDto>>> Get()
        {
            var recepciones = await _repository.GetAllAsync();
            var referenciasPedido = await _context.DetallesRecepciones
                .Select(d => new
                {
                    d.RecepcionId,
                    d.PedidoId,
                    FacturaPedido = d.Pedido == null ? null : d.Pedido.FacturaPedidos
                })
                .ToListAsync();
            var pedidosPorRecepcion = referenciasPedido
                .GroupBy(x => x.RecepcionId)
                .ToDictionary(g => g.Key, g => g.First());

            var dtos = recepciones.Select(recepcion =>
            {
                pedidosPorRecepcion.TryGetValue(recepcion.Id, out var referencia);

                return new RecepcionDto
                {
                    Id = recepcion.Id,
                    FechaDeEntrega = recepcion.FechaDeEntrega,
                    PrecioEnvioTotal = recepcion.PrecioEnvioTotal,
                    RemitoProveedor = recepcion.RemitoProveedor,
                    RecibidoPor = recepcion.RecibidoPor,
                    Notas = recepcion.Notas,
                    PedidoId = referencia?.PedidoId,
                    FacturaPedido = referencia?.FacturaPedido
                };
            });

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<RecepcionDto>> PostRecepcion([FromBody] RecepcionDto dto)
        {
            if (dto.PrecioEnvioTotal < 0)
            {
                return BadRequest("El costo total del envío no puede ser negativo.");
            }

            try
            {
                var fechaUtc = DateTime.SpecifyKind(dto.FechaDeEntrega, DateTimeKind.Utc);

                var recepcion = new Recepcion
                {
                    FechaDeEntrega = fechaUtc,
                    PrecioEnvioTotal = dto.PrecioEnvioTotal,
                    RemitoProveedor = dto.RemitoProveedor,
                    RecibidoPor = dto.RecibidoPor,
                    Notas = dto.Notas
                };

                var recepcionCreada = await _repository.AddRecepcionAsync(recepcion);

                dto.Id = recepcionCreada.Id;
                return Ok(dto);
            }
            catch (DbUpdateException ex)
            {
                var mensajeInterno = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(
                    500,
                    $"Error de base de datos al guardar la recepción: {mensajeInterno}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error general al registrar la recepción: {ex.Message}");
            }
        }

        [HttpGet("pendientes/{pedidoId}")]
        public async Task<ActionResult<IEnumerable<ItemPedidoPendienteDTO>>> GetPendientes(int pedidoId)
        {
            try
            {
                return Ok(await _repository.GetItemsPendientesPorPedidoAsync(pedidoId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
