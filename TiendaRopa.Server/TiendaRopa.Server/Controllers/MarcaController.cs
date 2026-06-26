using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Producto;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/Marca")]
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaRepositorio repositorio;

        public MarcaController(IMarcaRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("listamarca")] //api/Marca/listamarca
        public async Task<ActionResult<List<MarcaMostrarDTO>>> ListarMarcas()
        {
            var lista = await repositorio.SelectListaMarcas();
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

        [HttpGet("{id:int}")] //api/Marca/{id}
        public async Task<ActionResult<MarcaMostrarDTO>> GetById(int id)
        {
            var entidad = await repositorio.SelectById(id);
            if (entidad == null)
            {
                return NotFound($"No se existe el registro con ID: {id}.");
            }
            return Ok(entidad);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost] //api/Marca/Crear
        public async Task<ActionResult<int>> Post(MarcaCrearDTO DTO)
        {
            bool yaExiste = await repositorio.ExisteNombre(DTO.NombreMarca);
            if (yaExiste)
            {
                return BadRequest($"El registro '{DTO.NombreMarca}' ya se encuentra registrado.");
            }
            Marca entidad = new Marca
            {
                NombreMarca = DTO.NombreMarca
            };

            var id = await repositorio.Insert(entidad);

            return CreatedAtAction(nameof(GetById), new { id = entidad.Id }, entidad.Id); ;
        }

       // [Authorize(Roles = "Administrador")]
        [HttpPut("{id:int}")] //api/Marca/Actualizar/{id}
        public async Task<ActionResult> Put(int id, Marca DTO)
        {
            var flag = await repositorio.Update(id, DTO);
            if (!flag)
            {
                return BadRequest("Datos no validos o el registro no existe.");
            }
            return Ok($"Registro con el id: {id} actualizado correctamente.");
        }

       // [Authorize(Roles = "Administrador")]
        [HttpDelete("{id:int}")] //api/Marca/Eliminar/{id}
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
