using Microsoft.AspNetCore.Identity;
using TiendaRopa.BD.Datos;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Usuario;

namespace TiendaRopa.Repositorio.Repositorios.Usuario
{
    public interface IApplicationUserRepositorio 
  
    {
        Task<UsuarioMostrarDTO?> ObtenerPorIdAsync(string id);
        Task<ApplicationUser?> ObtenerEntidadStringId(string id);
        Task<IdentityResult> RegistrarUsuarioSeguro(UsuarioCrearDTO model);
        Task<List<UsuarioMostrarDTO>> SelectListaUsuarios();
        Task<IdentityResult> UpdateUsuario(ApplicationUser usuario);
    }
}