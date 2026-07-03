using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.ProductoRep;
using TiendaRopa.Shared.DTO.Producto_y_mas;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/ProductoColor")]
    public class ProductoColorController : ControllerBase
    {
        private readonly IProductoColorRepositorio repositorio;

        public ProductoColorController(IProductoColorRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }
        [HttpGet("lista-productos-colores-activos")] //api/ProductoColor/lista-productos-colores-activos
        public async Task<ActionResult<List<ProductoColorMostrarDTO>>> GetLista()
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

        [HttpGet("lista-productos-colores-inactivos")] //api/ProductoColor/lista-productos-colores-inactivos
        public async Task<ActionResult<List<ProductoColorMostrarDTO>>> GetListaInactivos()
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

        [HttpGet("{id:int}")] //api/ProductoColor/{id}
        public async Task<ActionResult<ProductoColorMostrarDTO>> GetById(int id)
        {
            var entidad = await repositorio.ObtenerById(id);
            if (entidad == null)
            {
                return NotFound($"No se existe el registro con ID: {id}.");
            }
            return Ok(entidad);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost("crear")] //api/ProductoColor/crear
        public async Task<ActionResult<int>> Post(ProductoColorCrearDTO DTO)
        {
           
            ProductoColor entidad = new ProductoColor
            {
                UrlImagen = DTO.UrlImagen,
                ProductoId = DTO.ProductoId,
                ColorId = DTO.ColorId,
                EstadoRegistro = DTO.Estado
            };

            var id = await repositorio.Insert(entidad);

            return CreatedAtAction(nameof(GetById), new { id = entidad.Id }, entidad.Id); ;
        }

        // [Authorize(Roles = "Administrador")]
        [HttpPut("editar/{id:int}")] //api/ProductoColor/editar/{id}
        public async Task<ActionResult> Put(int id, ProductoColorEditarDTO DTO)
        {
            var flag = await repositorio.Editar(id, DTO);
            if (!flag)
            {
                return BadRequest("Datos no validos o el registro no existe.");
            }
            return Ok($"Registro con el id: {id} actualizado correctamente.");
        }

        // [Authorize(Roles = "Administrador")]
        [HttpDelete("borrar/{id:int}")] //api/ProductoColor/Borrar/{id}
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
