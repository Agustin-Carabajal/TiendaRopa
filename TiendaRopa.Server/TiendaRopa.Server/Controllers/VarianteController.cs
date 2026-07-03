using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.ProductoRep;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/Variante")]
    public class VarianteController : ControllerBase
    {
        private readonly IVarianteRepositorio repositorio;

        public VarianteController(IVarianteRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }
        [HttpGet("lista-variantes-activas")] //api/Variante/lista-variantes-activas
        public async Task<ActionResult<List<VarianteMostrarDTO>>> GetLista()
        {
            var lista = await repositorio.GetListaActivos();
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

        [HttpGet("lista-variantes-inactivas")] //api/Variante/lista-variantes-inactivas
        public async Task<ActionResult<List<VarianteMostrarDTO>>> GetListaInactivos()
        {
            var lista = await repositorio.GetListaInactivos();
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

        [HttpGet("{id:int}")] //api/Variante/{id}
        public async Task<ActionResult<VarianteMostrarDTO>> GetById(int id)
        {
            var entidad = await repositorio.ObtenerById(id);
            if (entidad == null)
            {
                return NotFound($"No se existe el registro con ID: {id}.");
            }
            return Ok(entidad);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost("crear")] //api/Variante/crear
        public async Task<ActionResult<int>> Post(VarianteCrearDTO DTO)
        {
           
            Variante entidad = new Variante
            {
                CodVariante = DTO.CodVariante,
                Stock = DTO.Stock,
                PrecioVenta = DTO.PrecioVenta,
                ProductoColorId = DTO.ProductoColorId,
                TalleId = DTO.TalleId,
                EstadoRegistro = DTO.Estado
            };

            var id = await repositorio.Insert(entidad);

            return CreatedAtAction(nameof(GetById), new { id = entidad.Id }, entidad.Id); ;
        }

        // [Authorize(Roles = "Administrador")]
        [HttpPut("editar/{id:int}")] //api/Variante/editar/{id}
        public async Task<ActionResult> Put(int id, VarianteCrearDTO DTO)
        {
            var flag = await repositorio.Editar(id, DTO);
            if (!flag)
            {
                return BadRequest("Datos no validos o el registro no existe.");
            }
            return Ok($"Registro con el id: {id} actualizado correctamente.");
        }

        // [Authorize(Roles = "Administrador")]
        [HttpDelete("borrar/{id:int}")] //api/Variante/borrar/{id}
        public async Task<ActionResult> Delete(int id)
        {
            var flag = await repositorio.DeleteLogico(id);
            if (!flag)
            {
                return NotFound($"No existe el registro con el id: {id} o ya fue eliminado.");
            }
            return Ok($"Registro con el id: {id} eliminado correctamente.");
        }
    }
}
