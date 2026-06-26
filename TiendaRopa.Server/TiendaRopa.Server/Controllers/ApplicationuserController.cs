using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos;
using TiendaRopa.Repositorio.Repositorios.Usuario;
using TiendaRopa.Shared.DTO.Usuario;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/Usuarios")]
    public class ApplicationuserController : ControllerBase
    {
        private readonly IApplicationUserRepositorio repositorio;

        public ApplicationuserController(IApplicationUserRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet] //api/Usuarios
        public async Task<ActionResult<List<UsuarioMostrarDTO>>> GetListaUsuarios()
        {
            var lista = await repositorio.SelectListaUsuarios();
            if (lista == null || lista.Count == 0)
            {
                return NotFound("No se encontraron usuarios.");
            }
            return Ok(lista);
        }

        [HttpGet("{id}")] //api/Usuarios/{id}
        public async Task<ActionResult<UsuarioMostrarDTO>> GetUsuarioById(string id)
        {
            var usuario = await repositorio.ObtenerPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            return Ok(usuario);
        }

        [HttpPost("admin-crear")] //api/CrearUsuarios
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> AdminCrearUsuario([FromBody] UsuarioCrearDTO model)
        {
            var resultado = await repositorio.RegistrarUsuarioSeguro(model);
            if (!resultado.Succeeded)
            {
                return BadRequest("No se pudo registrar el usuario.");
            }
            return Ok(new { Mensaje = $"Usuario {model.Email} registrado correctamente como {model.Rol}." });
        }

        [HttpPost("registro")] //api/Usuarios/registro
        [AllowAnonymous]
        public async Task<ActionResult> RegistroUsuario([FromBody] UsuarioCrearDTO model)
        {
            model.Rol = "Cliente"; // Asignar el rol "Cliente" por defecto

            var resultado = await repositorio.RegistrarUsuarioSeguro(model);
            if (!resultado.Succeeded)
            {
                return BadRequest("No se pudo registrar el usuario.");
            }
            return Ok(new { Mensaje = $"Usuario {model.Email} registrado correctamente." });
        }

        [HttpPut("admin-actualizar/{id}")] //api/Usuarios/ActualizarByAdmin/{id})
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ActualizarUsuarioAdmin([FromRoute] string id, [FromBody] UsuarioActualizarAdminDTO model)
        {
            var usuarioExistente = await repositorio.ObtenerEntidadStringId(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            usuarioExistente.Nombre = model.Nombre;
            usuarioExistente.Apellido = model.Apellido;
            usuarioExistente.Dni = model.Dni;
            usuarioExistente.FechaNacimiento = model.FechaNacimiento;
            usuarioExistente.Direccion = model.Direccion;
            usuarioExistente.Saldo = model.Saldo;
            usuarioExistente.EstadoRegistro = model.EstadoRegistro;
            usuarioExistente.Observacion = model.Observacion;

            var resultado = await repositorio.UpdateUsuario(usuarioExistente);
            if (!resultado.Succeeded)
            {
                return BadRequest("No se pudo actualizar el usuario.");
            }
            return Ok(new { Mensaje = $"Usuario con ID {id} actualizado correctamente." });
        }

        [HttpPut("actualizar-mi-perfil/{id}")] //api/Usuarios/ActualizarPerfil/{id})
        [Authorize]
        public async Task<ActionResult> ActualizarUsuarioPerfil([FromRoute] string id, [FromBody] UsuarioActualizarPerfilDTO model)
        {
            var usuarioExistente = await repositorio.ObtenerEntidadStringId(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            usuarioExistente.Nombre = model.Nombre;
            usuarioExistente.Apellido = model.Apellido;
            usuarioExistente.Dni = model.Dni;
            usuarioExistente.FechaNacimiento = model.FechaNacimiento;
            usuarioExistente.Direccion = model.Direccion;
            var resultado = await repositorio.UpdateUsuario(usuarioExistente);
            if (!resultado.Succeeded)
            {
                return BadRequest("No se pudo actualizar el perfil del usuario.");
            }
            return Ok(new { Mensaje = $"Perfil del usuario con ID {id} actualizado correctamente." });
        }

        [HttpDelete("{id}")] //api/Usuarios/Borrar/{id}
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> BorrarUsuario(string id)
        {
            var usuarioExistente = await repositorio.ObtenerEntidadStringId(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            usuarioExistente.EstadoRegistro = EstadoRegistro.eliminado;
            usuarioExistente.Observacion = $"Registro inactivado por el sistema el {DateTime.Now:dd/MM/yyyy HH:mm}.";

            var resultado = await repositorio.UpdateUsuario(usuarioExistente);
            if (!resultado.Succeeded)
            {
                return BadRequest("No se pudo eliminar el usuario.");
            }
            return Ok(new { Mensaje = $"Usuario con ID {id} eliminado correctamente." });


        }
    }
}
