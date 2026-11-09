using Microsoft.EntityFrameworkCore;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Repositorio.Repositorios.ProductoRep
{
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly AppDbContext context;

        public MarcaRepositorio(AppDbContext context) : base(context) => this.context = context;

        public Task<List<MarcaMostrarDTO>> SelectListaMarcas() => context.Marcas
            .Where(m => m.EstadoRegistro != EstadoRegistro.eliminado && m.EstadoRegistro != EstadoRegistro.inactivo)
            .OrderBy(m => m.NombreMarca)
            .Select(m => new MarcaMostrarDTO { IdMarca = m.Id, NombreMarca = m.NombreMarca })
            .ToListAsync();

        public Task<bool> ExisteNombre(string nombre)
        {
            var nombreNormalizado = nombre.Trim().ToLower();
            return context.Marcas.AnyAsync(m => m.EstadoRegistro != EstadoRegistro.eliminado
                && m.NombreMarca.Trim().ToLower() == nombreNormalizado);
        }
    }
}
