using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos;
using TiendaRopa.Repositorio.Repositorios.Usuario;
using TiendaRopa.Shared.DTO.Usuario;

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
        public async Task<ActionResult<UsuarioMostrarDTO>> GetUsuarioById(int id)
        {
            var usuario = await repositorio.SelectById(id);
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
            if (!resultado)
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
            if (!resultado)
            {
                return BadRequest("No se pudo registrar el usuario.");
            }
            return Ok(new { Mensaje = $"Usuario {model.Email} registrado correctamente." });
        }

        [HttpPut("admin-actualizar/{id:int}")] //api/Usuarios/ActualizarByAdmin/{id})
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ActualizarUsuarioAdmin([FromRoute] int id, [FromBody] UsuarioActualizarAdminDTO model)
        {
            if (id != model.IntId) return BadRequest("Los IDs no coinciden.");
            var usuarioExistente = await repositorio.SelectById(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            usuarioExistente.NombreUsuario = model.NombreUsuario;
            usuarioExistente.ApellidoUsuario = model.ApellidoUsuario;
            usuarioExistente.DniUsuario = model.DniUsuario;
            usuarioExistente.FechaNacimientoUsuario = model.FechaNacimientoUsuario;
            usuarioExistente.DireccionUsuario = model.DireccionUsuario;
            usuarioExistente.SaldoUsuario = model.SaldoUsuario;
            usuarioExistente.EstadoRegistro = model.EstadoRegistro;
            usuarioExistente.Observacion = model.Observacion;

            var resultado = await repositorio.Update(id, usuarioExistente);
            if (!resultado)
            {
                return BadRequest("No se pudo actualizar el usuario.");
            }
            return Ok(new { Mensaje = $"Usuario con ID {id} actualizado correctamente." });
        }

        [HttpPut("actualizar-mi-perfil/{id:int}")] //api/Usuarios/ActualizarPerfil  /{id})
        [Authorize]
        public async Task<ActionResult> ActualizarUsuarioPerfil([FromRoute] int id, [FromBody] UsuarioActualizarPerfilDTO model)
        {
            if (id != model.IntId) return BadRequest("Los IDs no coinciden.");
            var usuarioExistente = await repositorio.SelectById(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            usuarioExistente.NombreUsuario = model.NombreUsuario;
            usuarioExistente.ApellidoUsuario = model.ApellidoUsuario;
            usuarioExistente.DniUsuario = model.DniUsuario;
            usuarioExistente.FechaNacimientoUsuario = model.FechaNacimientoUsuario;
            usuarioExistente.DireccionUsuario = model.DireccionUsuario;
            var resultado = await repositorio.Update(id, usuarioExistente);
            if (!resultado)
            {
                return BadRequest("No se pudo actualizar el perfil del usuario.");
            }
            return Ok(new { Mensaje = $"Perfil del usuario con ID {id} actualizado correctamente." });
        }

        [HttpDelete("{id:int}")] //api/Usuarios/Borrar/{id}
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> BorrarUsuario(int id)
        {
            var usuarioExistente = await repositorio.SelectById(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            var resultado = await repositorio.DeleteLogico(id);
            if (!resultado)
            {
                return BadRequest("No se pudo eliminar el usuario.");
            }
            return Ok(new { Mensaje = $"Usuario con ID {id} eliminado correctamente." });

        }
    }
}
