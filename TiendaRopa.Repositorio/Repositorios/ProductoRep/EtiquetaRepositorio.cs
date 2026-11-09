using Microsoft.EntityFrameworkCore;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Repositorio.Repositorios.ProductoRep
{
    public class EtiquetaRepositorio : Repositorio<Etiqueta>, IEtiquetaRepositorio
    {
        private readonly AppDbContext context;

        public EtiquetaRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<EtiquetaMostrarDTO>> SelectListaEtiquetas()
        {
            return await context.Etiquetas
                .Where(e => e.EstadoRegistro != EstadoRegistro.eliminado && e.EstadoRegistro != EstadoRegistro.inactivo)
                .Select(e => new EtiquetaMostrarDTO { IdEtiqueta = e.Id, Nombre = e.Nombre })
                .ToListAsync();
        }

        public async Task<bool> ExisteNombre(string nombre)
        {
            var nombreNormalizado = nombre.Trim().ToLower();
            return await context.Etiquetas.AnyAsync(e =>
                e.EstadoRegistro != EstadoRegistro.eliminado && e.Nombre.Trim().ToLower() == nombreNormalizado);
        }
    }
}
