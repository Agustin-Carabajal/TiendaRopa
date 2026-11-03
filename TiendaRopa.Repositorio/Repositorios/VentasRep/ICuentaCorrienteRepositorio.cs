using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Venta;
namespace TiendaRopa.Repositorio.Repositorios.VentasRep;
public interface ICuentaCorrienteRepositorio : IRepositorio<MovimientoCuentaCorriente> { Task<List<CuentaCorrienteMovimientoDTO>> Historial(int clienteId); Task<bool> RegistrarPago(int clienteId, decimal monto); }
