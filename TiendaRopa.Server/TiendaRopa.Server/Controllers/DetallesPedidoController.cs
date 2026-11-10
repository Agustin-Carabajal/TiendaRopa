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
            return Ok(detalles.Select(MapearDetalle));
        }

        [HttpGet("porpedido/{pedidoId:int}")]
        public async Task<ActionResult<IEnumerable<DetallePedidoDTO>>> GetPorPedido(int pedidoId)
        {
            var detalles = await _repository.GetAllAsync();
            return Ok(detalles.Where(d => d.PedidoId == pedidoId).OrderBy(d => d.Id).Select(MapearDetalle));
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
                PedidoId = d.PedidoId,
                VarianteId = d.VarianteId,
                NombreProducto = NombreVariante(d.Variante),
                CodVariante = d.Variante?.CodVariante,
                Color = d.Variante?.ProductoColor?.Color?.NombreColor,
                Talle = d.Variante?.Talle?.NombreTalle
            };
            return Ok(dto);
        }

        private static string? NombreVariante(Variante? variante)
        {
            if (variante == null) return null;
            var producto = variante.ProductoColor?.Producto?.NombreProducto ?? "Producto";
            var color = variante.ProductoColor?.Color?.NombreColor ?? "Color sin asignar";
            var talle = variante.Talle?.NombreTalle ?? "Talle sin asignar";
            return $"{producto} - {color} - Talle {talle} - SKU {variante.CodVariante}";
        }

        private static DetallePedidoDTO MapearDetalle(DetallesPedido detalle) => new()
        {
            Id = detalle.Id,
            Cant_prod_Pedido = detalle.Cant_prod_Pedido,
            PedidoId = detalle.PedidoId,
            FacturaPedido = detalle.Pedido?.FacturaPedidos,
            VarianteId = detalle.VarianteId,
            NombreProducto = NombreVariante(detalle.Variante),
            CodVariante = detalle.Variante?.CodVariante,
            Color = detalle.Variante?.ProductoColor?.Color?.NombreColor,
            Talle = detalle.Variante?.Talle?.NombreTalle
        };

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DetallePedidoDTO dto)
        {
            var detalle = new DetallesPedido
            {
                Cant_prod_Pedido = dto.Cant_prod_Pedido,
                Valor_est = 0m,
                Valor_uni = 0m,
                PedidoId = dto.PedidoId,
                VarianteId = dto.VarianteId // <-- Vinculado directamente a la variante seleccionada
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
            d.Valor_est = 0m;
            d.Valor_uni = 0m;
            d.PedidoId = dto.PedidoId;
            d.VarianteId = dto.VarianteId;

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
