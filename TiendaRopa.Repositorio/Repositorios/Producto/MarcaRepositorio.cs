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
    public class MarcaRepositorio : Repositorio<Marca>, IRepositorio<Marca>, IMarcaRepositorio
    {
        private readonly AppDbContext context;

        public MarcaRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<MarcaMostrarDTO>> SelectListaMarcas()
        {
            var lista = await context.Marcas.Select(x => new MarcaMostrarDTO
            {
                IdMarca = x.Id,
                NombreMarca = x.NombreMarca
            }).ToListAsync();
            return lista;
        }
    }
}
