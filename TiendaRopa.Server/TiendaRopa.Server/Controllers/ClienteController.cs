using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.ClientesRep;
using TiendaRopa.Shared.DTO.Cliente;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/Cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepositorio repositorio;

        public ClienteController(IClienteRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("listarclientes")] //api/Cliente/listarclientes
        public async Task<ActionResult<List<ClienteMostrarDTO>>> GetLista()
        {
            var lista = await repositorio.SelectListaClientes();
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

        [HttpGet("{id:int}")] //api/Cliente/{id}
        public async Task<ActionResult<ClienteMostrarDTO>> GetById(int id)
        {
            var entidad = await repositorio.ObtenerById(id);
            if (entidad == null)
            {
                return NotFound($"No se existe el registro con ID: {id}.");
            }
            return Ok(entidad);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost] //api/Cliente/Crear
        public async Task<ActionResult<int>> Post(ClienteCrearDTO DTO)
        {
            bool yaExiste = await repositorio.ExisteNombre(DTO.Nombre, DTO.Apellido!);
            if (yaExiste)
            {
                return BadRequest($"El registro '{DTO.Nombre} {DTO.Apellido}' ya se encuentra registrado.");
            }
            Cliente entidad = new Cliente
            {
                Nombre = DTO.Nombre,
                Apellido = DTO.Apellido,
                Dni = DTO.Dni,
                FechaNacimiento = DTO.FechaNacimiento,
                Domicilio = DTO.Domicilio,
                Telefono = DTO.Telefono,
                Saldo = DTO.Saldo,
                Rol = DTO.Rol
            };

            var id = await repositorio.Insert(entidad);

            return CreatedAtAction(nameof(GetById), new { id = entidad.Id }, entidad.Id); ;
        }

       // [Authorize(Roles = "Administrador")]
        [HttpPut("editar/{id:int}")] //api/Cliente/editar/{id}
        public async Task<ActionResult> Put(int id, Cliente DTO)
        {
            bool yaExiste = await repositorio.ExisteNombre(DTO.Nombre, DTO.Apellido!);
            if (yaExiste)
            {
                return BadRequest($"El cliente '{DTO.Nombre} {DTO.Apellido}' ya se encuentra registrado.");
            }
            var flag = await repositorio.Update(id, DTO);
            if (!flag)
            {
                return BadRequest("Datos no validos o el registro no existe.");
            }
            return Ok($"Registro con el id: {id} actualizado correctamente.");
        }

       // [Authorize(Roles = "Administrador")]
        [HttpDelete("borrar/{id:int}")] //api/Cliente/Borrar/{id}
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
