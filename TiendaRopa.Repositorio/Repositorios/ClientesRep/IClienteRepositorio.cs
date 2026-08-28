using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Cliente;

namespace TiendaRopa.Repositorio.Repositorios.ClientesRep
{
    public interface IClienteRepositorio : IRepositorio<Cliente>
    {
        Task<bool> ExisteNombre(string nombre, string apellido);
        Task<ClienteMostrarDTO?> ObtenerById(int id);
        Task<List<ClienteMostrarDTO>> SelectListaClientes();
    }
}