using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;
using Microsoft.EntityFrameworkCore;

namespace TiendaRopa.Repositorio.Repositorios.ProductoRep
{
    public class TalleRepositorio : Repositorio<Talle>, IRepositorio<Talle>, ITalleRepositorio
    {
        private readonly AppDbContext context;

        public TalleRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<TalleMostrarDTO>> SelectListaTalles()
        {
            var lista = await context.Talles.Select(x => new TalleMostrarDTO
            {
                IdTalle = x.Id,
                NombreTalle = x.NombreTalle
            }).ToListAsync();
            return lista;
        }

        public async Task<bool> ExisteNombre(string nombre)
        {
            // Busca en la base de datos si hay coincidencia exacta sin importar mayúsculas
            return await context.Set<Talle>()
                .AnyAsync(m => m.NombreTalle.ToLower().Trim() == nombre.ToLower().Trim());
        }
    }
}
