using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;
using Microsoft.EntityFrameworkCore;

namespace TiendaRopa.Repositorio.Repositorios.Producto
{
    public class ColorRepositorio : Repositorio<Color>, IRepositorio<Color>, IColorRepositorio
    {
        private readonly AppDbContext context;

        public ColorRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<ColorMostrarDTO>> SelectListaColores()
        {
            var lista = await context.Colores.Select(x => new ColorMostrarDTO
            {
                IdColor = x.Id,
                NombreColor = x.NombreColor
            }).ToListAsync();
            return lista;
        }

        public async Task<bool> ExisteNombre(string nombre)
        {
            // Busca en la base de datos si hay coincidencia exacta sin importar mayúsculas
            return await context.Set<Color>()
                .AnyAsync(m => m.NombreColor.ToLower().Trim() == nombre.ToLower().Trim());
        }
}
}
