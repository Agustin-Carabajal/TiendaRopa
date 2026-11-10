using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.PedidosRep;
using TiendaRopa.Server.Services;
using TiendaRopa.Shared.DTO.Proveedor;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoRepositorio _repository;
        private readonly IDetallePedidoRepositorio _detallePedidoRepositorio;

        public PedidosController(IPedidoRepositorio repository, IDetallePedidoRepositorio detallePedidoRepositorio)
        {
            _repository = repository;
            _detallePedidoRepositorio = detallePedidoRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> Get()
        {
            var pedidos = await _repository.GetAllAsync();
            var dtos = pedidos.Select(p => new PedidoDTO
            {
                Id = p.Id,
                TotalPedido = p.TotalPedidos,
                FechaDePedido = p.FechaDePedido,
                FechaDeEntrega = p.FechaDeEntrega,
                FacturaPedido = p.FacturaPedidos,
                IdProveedor = p.ProveedorId,
                RazonSocialProveedor = p.Proveedor?.RazonSocialProveedores
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDTO>> Get(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null)
            {
                return NotFound();
            }

            var dto = new PedidoDTO
            {
                Id = p.Id,
                TotalPedido = p.TotalPedidos,
                FechaDePedido = p.FechaDePedido,
                FechaDeEntrega = p.FechaDeEntrega,
                FacturaPedido = p.FacturaPedidos,
                IdProveedor = p.ProveedorId
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoDTO>> Post([FromBody] PedidoDTO dto)
        {
            if (dto == null || dto.IdProveedor <= 0)
            {
                return BadRequest("El proveedor es obligatorio.");
            }

            var nuevoPedido = new Pedido
            {
                ProveedorId = dto.IdProveedor,
                FacturaPedidos = string.IsNullOrWhiteSpace(dto.FacturaPedido)
                                    ? $"PED-{DateTime.Now:yyyyMMdd-HHmm}"
                                    : dto.FacturaPedido,
                FechaDePedido = dto.FechaDePedido,
                FechaDeEntrega = dto.FechaDeEntrega,
                TotalPedidos = 0m
            };

            await _repository.AddAsync(nuevoPedido);

            dto.Id = nuevoPedido.Id;
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PedidoDTO dto)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null)
            {
                return NotFound();
            }

            p.FechaDePedido = dto.FechaDePedido;
            p.FechaDeEntrega = dto.FechaDeEntrega;
            p.FacturaPedidos = dto.FacturaPedido;
            p.ProveedorId = dto.IdProveedor;

            await _repository.UpdateAsync(p);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id:int}/recibo.pdf")]
        public async Task<IActionResult> DescargarReciboPdf(int id)
        {
            var pedidos = await _repository.GetAllAsync();
            var pedido = pedidos.FirstOrDefault(p => p.Id == id);
            if (pedido == null) return NotFound("No se encontró el pedido.");

            var detalles = (await _detallePedidoRepositorio.GetAllAsync())
                .Where(d => d.PedidoId == id)
                .OrderBy(d => d.Id)
                .ToList();
            if (detalles.Count == 0)
            {
                return NotFound("El pedido no tiene artículos para incluir en el recibo.");
            }

            var pdf = PedidoPdfGenerator.Generar(
                pedido,
                pedido.Proveedor?.RazonSocialProveedores ?? "Sin proveedor",
                detalles);
            var nombreArchivo = new string(pedido.FacturaPedidos
                .Where(c => char.IsLetterOrDigit(c) || c is '-' or '_')
                .ToArray());

            var identificadorArchivo = string.IsNullOrWhiteSpace(nombreArchivo)
                ? id.ToString()
                : nombreArchivo;

            return File(
                pdf,
                "application/pdf",
                $"Recibo-{identificadorArchivo}.pdf");
        }
    }
}
