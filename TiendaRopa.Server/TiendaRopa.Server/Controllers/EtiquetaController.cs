using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.ProductoRep;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/Etiqueta")]
    public class EtiquetaController : ControllerBase
    {
        private readonly IEtiquetaRepositorio repositorio;

        public EtiquetaController(IEtiquetaRepositorio repositorio) => this.repositorio = repositorio;

        [HttpGet("listaetiquetas")]
        public async Task<ActionResult<List<EtiquetaMostrarDTO>>> GetLista()
        {
            var lista = await repositorio.SelectListaEtiquetas();
            return Ok(lista);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EtiquetaMostrarDTO>> GetById(int id)
        {
            var entidad = await repositorio.SelectById(id);
            if (entidad == null) return NotFound($"No existe el registro con ID: {id}.");
            return Ok(new EtiquetaMostrarDTO { IdEtiqueta = entidad.Id, Nombre = entidad.Nombre });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(EtiquetaCrearDTO dto)
        {
            if (await repositorio.ExisteNombre(dto.Nombre))
                return BadRequest($"El registro '{dto.Nombre}' ya se encuentra registrado.");

            var entidad = new Etiqueta { Nombre = dto.Nombre.Trim(), EstadoRegistro = TiendaRopa.Shared.ENUM.EstadoRegistro.activo };
            await repositorio.Insert(entidad);
            return CreatedAtAction(nameof(GetById), new { id = entidad.Id }, entidad.Id);
        }

        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Put(int id, Etiqueta dto)
        {
            var entidad = await repositorio.SelectById(id);
            if (entidad == null) return BadRequest("Datos no válidos o el registro no existe.");
            if (await repositorio.ExisteNombre(dto.Nombre))
            {
                if (!string.Equals(entidad.Nombre.Trim(), dto.Nombre.Trim(), StringComparison.OrdinalIgnoreCase))
                    return BadRequest($"El registro '{dto.Nombre}' ya se encuentra registrado.");
            }
            entidad.Nombre = dto.Nombre.Trim();
            var flag = await repositorio.Update(id, entidad);
            return flag ? Ok($"Registro con el id: {id} actualizado correctamente.") : BadRequest("Datos no válidos o el registro no existe.");
        }

        [HttpDelete("borrar/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var flag = await repositorio.DeleteLogico(id);
            return flag ? Ok($"Registro con el id: {id} eliminado correctamente.") : NotFound($"No existe el registro con el id: {id} o ya fue eliminado.");
        }
    }
}
