using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.PedidosRep;
using TiendaRopa.Shared.DTO.Proveedor;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedorRepositorio _repository;

        public ProveedoresController(IProveedorRepositorio repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorDto>>> Get()
        {
            var proveedores = await _repository.GetAllAsync();
            var dtos = proveedores.Select(p => new ProveedorDto
            {
                Id = p.Id,
                RazonSocialProveedores = p.RazonSocialProveedores,
                CuitProveedores = p.CuitProveedores,
                DomicilioProveedores = p.DomicilioProveedores,
                ContactoNombreProveedores = p.ContactoNombreProveedores,
                EmailProveedores = p.EmailProveedores,
                ObvsProveedores = p.ObvsProveedores
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorDto>> Get(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null) return NotFound();

            var dto = new ProveedorDto
            {
                Id = p.Id,
                RazonSocialProveedores = p.RazonSocialProveedores,
                CuitProveedores = p.CuitProveedores,
                DomicilioProveedores = p.DomicilioProveedores,
                ContactoNombreProveedores = p.ContactoNombreProveedores,
                EmailProveedores = p.EmailProveedores,
                ObvsProveedores = p.ObvsProveedores
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProveedorDto dto)
        {
            var proveedor = new Proveedor
            {
                RazonSocialProveedores = dto.RazonSocialProveedores,
                CuitProveedores = dto.CuitProveedores,
                DomicilioProveedores = dto.DomicilioProveedores,
                ContactoNombreProveedores = dto.ContactoNombreProveedores,
                EmailProveedores = dto.EmailProveedores,
                ObvsProveedores = dto.ObvsProveedores
            };

            await _repository.AddAsync(proveedor);
            return CreatedAtAction(nameof(Get), new { id = proveedor.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProveedorDto dto)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null) return NotFound();

            p.RazonSocialProveedores = dto.RazonSocialProveedores;
            p.CuitProveedores = dto.CuitProveedores;
            p.DomicilioProveedores = dto.DomicilioProveedores;
            p.ContactoNombreProveedores = dto.ContactoNombreProveedores;
            p.EmailProveedores = dto.EmailProveedores;
            p.ObvsProveedores = dto.ObvsProveedores;

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
