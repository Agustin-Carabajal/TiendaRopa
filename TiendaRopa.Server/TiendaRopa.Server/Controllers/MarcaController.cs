using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.ProductoRep;
using TiendaRopa.Shared.DTO.Producto_y_mas;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/Marca")]
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaRepositorio repositorio;

        public MarcaController(IMarcaRepositorio repositorio) => this.repositorio = repositorio;

        [HttpGet("listamarcas")]
        public async Task<ActionResult<List<MarcaMostrarDTO>>> GetLista()
        {
            var marcas = await repositorio.SelectListaMarcas();
            return Ok(marcas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MarcaMostrarDTO>> GetById(int id)
        {
            var marca = await repositorio.SelectById(id);
            if (marca == null || marca.EstadoRegistro == EstadoRegistro.eliminado)
                return NotFound($"No existe una marca activa con ID {id}.");

            return Ok(new MarcaMostrarDTO { IdMarca = marca.Id, NombreMarca = marca.NombreMarca });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(MarcaCrearDTO dto)
        {
            var nombre = dto.NombreMarca.Trim();
            if (await repositorio.ExisteNombre(nombre))
                return BadRequest($"La marca '{nombre}' ya se encuentra registrada.");

            var marca = new Marca { NombreMarca = nombre, EstadoRegistro = EstadoRegistro.activo };
            await repositorio.Insert(marca);
            return CreatedAtAction(nameof(GetById), new { id = marca.Id }, marca.Id);
        }

        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Put(int id, MarcaCrearDTO dto)
        {
            var marca = await repositorio.SelectById(id);
            if (marca == null || marca.EstadoRegistro == EstadoRegistro.eliminado)
                return NotFound($"No existe una marca activa con ID {id}.");

            var nombre = dto.NombreMarca.Trim();
            if (await repositorio.ExisteNombre(nombre)
                && !string.Equals(marca.NombreMarca.Trim(), nombre, StringComparison.OrdinalIgnoreCase))
                return BadRequest($"La marca '{nombre}' ya se encuentra registrada.");

            marca.NombreMarca = nombre;
            var actualizado = await repositorio.Update(id, marca);
            return actualizado ? Ok($"Marca con ID {id} actualizada correctamente.") : BadRequest("No se pudo actualizar la marca.");
        }

        [HttpDelete("borrar/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var eliminada = await repositorio.DeleteLogico(id);
            return eliminada ? Ok($"Marca con ID {id} eliminada correctamente.") : NotFound($"No existe la marca con ID {id} o ya fue eliminada.");
        }
    }
}
