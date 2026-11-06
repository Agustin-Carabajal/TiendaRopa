using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
namespace TiendaRopa.Repositorio.Repositorios.VentasRep;
public interface IPagoRepositorio : IRepositorio<Pago> { Task<List<Pago>> ListarActivos(); }
