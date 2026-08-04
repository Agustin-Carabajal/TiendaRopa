using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.PedidosRep;
using TiendaRopa.Shared.DTO.Proveedor;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetallesPedidoController : ControllerBase
    {
        private readonly IDetallePedidoRepositorio _repository;

        public DetallesPedidoController(IDetallePedidoRepositorio repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedidoDTO>>> Get()
        {
            var detalles = await _repository.GetAllAsync();
            var dtos = detalles.Select(d => new DetallePedidoDTO
            {
                Id = d.Id,
                Cant_prod_Pedido = d.Cant_prod_Pedido,
                Valor_est = d.Valor_est,
                Valor_uni = d.Valor_uni,
                PedidoId = d.PedidoId,
                FacturaPedido = d.Pedido?.FacturaPedidos, 
                ProductoId = d.ProductoId,
                NombreProducto = d.Producto?.NombreProducto 
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePedidoDTO>> Get(int id)
        {
            var d = await _repository.GetByIdAsync(id);
            if (d == null) return NotFound();

            var dto = new DetallePedidoDTO
            {
                Id = d.Id,
                Cant_prod_Pedido = d.Cant_prod_Pedido,
                Valor_est = d.Valor_est,
                Valor_uni = d.Valor_uni,
                PedidoId = d.PedidoId,
                ProductoId = d.ProductoId
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DetallePedidoDTO dto)
        {
            var detalle = new DetallesPedido
            {
                Cant_prod_Pedido = dto.Cant_prod_Pedido,
                Valor_est = dto.Valor_est,
                Valor_uni = dto.Valor_uni,
                PedidoId = dto.PedidoId,
                ProductoId = dto.ProductoId
            };

            await _repository.AddAsync(detalle);
            return CreatedAtAction(nameof(Get), new { id = detalle.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] DetallePedidoDTO dto)
        {
            var d = await _repository.GetByIdAsync(id);
            if (d == null) return NotFound();

            d.Cant_prod_Pedido = dto.Cant_prod_Pedido;
            d.Valor_est = dto.Valor_est;
            d.Valor_uni = dto.Valor_uni;
            d.PedidoId = dto.PedidoId;
            d.ProductoId = dto.ProductoId;

            await _repository.UpdateAsync(d);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var d = await _repository.GetByIdAsync(id);
            if (d == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
