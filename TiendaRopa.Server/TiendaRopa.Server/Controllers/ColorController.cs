using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Producto;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/Color")]
    public class ColorController : ControllerBase
    {
        private readonly IColorRepositorio repositorio;

        public ColorController(IColorRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("listacolores")] //api/Color/listacolores
        public async Task<ActionResult<List<ColorMostrarDTO>>> GetLista()
        {
            var lista = await repositorio.SelectListaColores();
            if (lista == null)
            {
                return NotFound("No se encontraron elementos de la lista, VERIFICAR.");
            }
            if (lista.Count == 0)
            {
                return NotFound("Lista sin registros.");
            }
            return Ok(lista);
        }

        [HttpGet("{id:int}")] //api/Color/{id}
        public async Task<ActionResult<ColorMostrarDTO>> GetById(int id)
        {
            var entidad = await repositorio.SelectById(id);
            if (entidad == null)
            {
                return NotFound($"No se existe el registro con ID: {id}.");
            }
            return Ok(entidad);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost] //api/Color/Crear
        public async Task<ActionResult<int>> Post(ColorCrearDTO DTO)
        {
            bool yaExiste = await repositorio.ExisteNombre(DTO.NombreColor);
            if (yaExiste)
            {
                return BadRequest($"El registro '{DTO.NombreColor}' ya se encuentra registrado.");
            }
            Color entidad = new Color
            {
                NombreColor = DTO.NombreColor
            };

            var id = await repositorio.Insert(entidad);

            return CreatedAtAction(nameof(GetById), new { id = entidad.Id }, entidad.Id); ;
        }

       // [Authorize(Roles = "Administrador")]
        [HttpPut("{id:int}")] //api/Color/Actualizar/{id}
        public async Task<ActionResult> Put(int id, Color DTO)
        {
            var flag = await repositorio.Update(id, DTO);
            if (!flag)
            {
                return BadRequest("Datos no validos o el registro no existe.");
            }
            return Ok($"Registro con el id: {id} actualizado correctamente.");
        }

       // [Authorize(Roles = "Administrador")]
        [HttpDelete("{id:int}")] //api/Talle/Eliminar/{id}
        public async Task<ActionResult> Delete(int id)
        {
            var flag = await repositorio.Delete(id);
            if (!flag)
            {
                return NotFound($"No existe el registro con el id: {id} o ya fue eliminado.");
            }
            return Ok($"Registro con el id: {id} eliminado correctamente.");
        }
    }
}
