using Microsoft.EntityFrameworkCore;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Venta;
using TiendaRopa.Shared.ENUM;
namespace TiendaRopa.Repositorio.Repositorios.VentasRep;
public class CuentaCorrienteRepositorio : Repositorio<MovimientoCuentaCorriente>, IRepositorio<MovimientoCuentaCorriente>, ICuentaCorrienteRepositorio { private readonly AppDbContext db; public CuentaCorrienteRepositorio(AppDbContext db):base(db)=>this.db=db; public async Task<List<CuentaCorrienteMovimientoDTO>> Historial(int id){var saldo=await db.Clientes.Where(c=>c.Id==id).Select(c=>(decimal?)c.Saldo).FirstOrDefaultAsync();if(saldo==null)return new();return await db.MovimientosCuentaCorriente.AsNoTracking().Where(m=>m.ClienteId==id).OrderByDescending(m=>m.Fecha).Select(m=>new CuentaCorrienteMovimientoDTO{ClienteId=id,Saldo=saldo.Value,Fecha=m.Fecha,Tipo=m.Tipo.ToString(),Monto=m.Monto,VentaId=m.VentaId}).ToListAsync();} public async Task<bool> RegistrarPago(int id,decimal monto){if(monto<=0)return false;var c=await db.Clientes.FirstOrDefaultAsync(x=>x.Id==id);if(c==null)return false;c.Saldo+=monto;db.MovimientosCuentaCorriente.Add(new MovimientoCuentaCorriente{ClienteId=id,Fecha=DateTime.UtcNow,Tipo=TipoMovimientoCuentaCorriente.Pago,Monto=monto,EstadoRegistro=EstadoRegistro.activo});await db.SaveChangesAsync();return true;} }
