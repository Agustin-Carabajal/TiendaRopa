using Microsoft.EntityFrameworkCore;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.ENUM;
namespace TiendaRopa.Repositorio.Repositorios.VentasRep;
public class PagoRepositorio : Repositorio<Pago>, IRepositorio<Pago>, IPagoRepositorio { private readonly AppDbContext db; public PagoRepositorio(AppDbContext db):base(db)=>this.db=db; public Task<List<Pago>> ListarActivos()=>db.Pagos.AsNoTracking().Where(p=>p.EstadoRegistro==EstadoRegistro.activo).OrderBy(p=>p.Tipo).ToListAsync(); }
