using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Producto;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/Talle")]
    public class TalleController : ControllerBase
    {
        private readonly ITalleRepositorio repositorio;

        public TalleController(ITalleRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("listatalles")] //api/Talle/listatalles
        public async Task<ActionResult<List<TalleMostrarDTO>>> GetLista()
        {
            var lista = await repositorio.SelectListaTalles();
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

        [HttpGet("{id:int}")] //api/Talle/{id}
        public async Task<ActionResult<TalleMostrarDTO>> GetById(int id)
        {
            var entidad = await repositorio.SelectById(id);
            if (entidad == null)
            {
                return NotFound($"No se existe el registro con ID: {id}.");
            }
            return Ok(entidad);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost] //api/Talle/Crear
        public async Task<ActionResult<int>> Post(TalleCrearDTO DTO)
        {
            bool yaExiste = await repositorio.ExisteNombre(DTO.NombreTalle);
            if (yaExiste)
            {
                return BadRequest($"El registro '{DTO.NombreTalle}' ya se encuentra registrado.");
            }
            Talle entidad = new Talle
            {
                NombreTalle = DTO.NombreTalle
            };

            var id = await repositorio.Insert(entidad);

            return CreatedAtAction(nameof(GetById), new { id = entidad.Id }, entidad.Id); ;
        }

       // [Authorize(Roles = "Administrador")]
        [HttpPut("{id:int}")] //api/Talle/Actualizar/{id}
        public async Task<ActionResult> Put(int id, Talle DTO)
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
