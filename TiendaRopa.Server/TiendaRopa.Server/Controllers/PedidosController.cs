using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.PedidosRep;
using TiendaRopa.Shared.DTO.Proveedor;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoRepositorio _repository;

        public PedidosController(IPedidoRepositorio repository)
        {
            _repository = repository;
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
                RazonSocialProveedor = p.Proveedor?.RazonSocialProveedores // Mapeamos el nombre
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDTO>> Get(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null) return NotFound();

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
        public async Task<IActionResult> Post([FromBody] PedidoDTO dto)
        {
            var pedido = new Pedido
            {
                TotalPedidos = dto.TotalPedido,
                FechaDePedido = dto.FechaDePedido,
                FechaDeEntrega = dto.FechaDeEntrega,
                FacturaPedidos = dto.FacturaPedido,
                ProveedorId = dto.IdProveedor
            };

            await _repository.AddAsync(pedido);
            return CreatedAtAction(nameof(Get), new { id = pedido.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PedidoDTO dto)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null) return NotFound();

            p.TotalPedidos = dto.TotalPedido;
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
            if (p == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
