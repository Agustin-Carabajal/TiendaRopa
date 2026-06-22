using TiendaRopa.BD.Datos;
using TiendaRopa.Shared.DTO.ApplicationUser;

namespace TiendaRopa.Repositorio.Repositorios
{
    public interface IApplicationUserRepositorio : IRepositorio<ApplicationUser>
  
    {
        Task<bool> RegistrarUsuarioSeguro(UsuarioCrearDTO model);
        Task<List<UsuarioMostrarDTO>> SelectListaUsuarios();
    }
}