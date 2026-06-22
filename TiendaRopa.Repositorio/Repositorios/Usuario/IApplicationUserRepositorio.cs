using TiendaRopa.BD.Datos;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Usuario;

namespace TiendaRopa.Repositorio.Repositorios.Usuario
{
    public interface IApplicationUserRepositorio : IRepositorio<ApplicationUser>
  
    {
        Task<bool> RegistrarUsuarioSeguro(UsuarioCrearDTO model);
        Task<List<UsuarioMostrarDTO>> SelectListaUsuarios();
    }
}