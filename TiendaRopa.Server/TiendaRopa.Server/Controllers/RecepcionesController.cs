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
        private readonly IPedidoRepositorio _pedidoRepositorio; // <-- Inyectamos el repositorio de pedidos
        private readonly AppDbContext _context; // Lo conservamos por ahora si consultas DetallesRecepciones directamente

        public RecepcionesController(AppDbContext context, IRecepcionRepositorio repository, IPedidoRepositorio pedidoRepositorio)
        {
            _context = context;
            _repository = repository;
            _pedidoRepositorio = pedidoRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecepcionDto>>> Get()
        {
            var recepciones = await _repository.GetAllAsync();
            var dtos = recepciones.Select(r => new RecepcionDto
            {
                Id = r.Id,
                FechaDeEntrega = r.FechaDeEntrega,
                RemitoProveedor = r.RemitoProveedor,
                RecibidoPor = r.RecibidoPor,
                Notas = r.Notas
            });

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<RecepcionDto>> PostRecepcion([FromBody] RecepcionDto dto)
        {
            try
            {
                // Aseguramos que la fecha sea compatible con UTC si usas PostgreSQL
                var fechaUtc = DateTime.SpecifyKind(dto.FechaDeEntrega, DateTimeKind.Utc);

                var recepcion = new Recepcion
                {
                    FechaDeEntrega = fechaUtc,
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
                // Extraemos el mensaje interno real de la base de datos (clave duplicada, campo nulo, etc.)
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"Error de base de datos al guardar la recepción: {innerMessage}");
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
                // 1. Obtenemos los detalles del pedido usando el repositorio limpio
                var detallesPedido = await _pedidoRepositorio.GetDetallesByPedidoIdAsync(pedidoId);

                if (detallesPedido == null || !detallesPedido.Any())
                {
                    return Ok(new List<ItemPedidoPendienteDTO>());
                }

                // 2. Obtenemos las recepciones anteriores para este pedido
                var recepcionesAnteriores = await _context.DetallesRecepciones
                    .Where(dr => dr.PedidoId == pedidoId)
                    .ToListAsync();

                var resultado = new List<ItemPedidoPendienteDTO>();

                foreach (var dp in detallesPedido)
                {
                    int yaRecibido = 0;

                    if (recepcionesAnteriores != null && recepcionesAnteriores.Any())
                    {
                        yaRecibido = recepcionesAnteriores
                            .Where(dr => dr.VarianteId == dp.VarianteId)
                            .Sum(dr => dr.CantidadRecibida);
                    }

                    int pendiente = dp.Cant_prod_Pedido - yaRecibido;

                    resultado.Add(new ItemPedidoPendienteDTO
                    {
                        VarianteId = dp.VarianteId,
                        NombreProducto = $"Variante ID: {dp.VarianteId}",
                        CantidadPedida = dp.Cant_prod_Pedido,
                        CantidadYaRecibida = yaRecibido,
                        PrecioUnitario = dp.Valor_uni,
                        CantidadARecibirAhora = pendiente > 0 ? pendiente : 0
                    });
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
