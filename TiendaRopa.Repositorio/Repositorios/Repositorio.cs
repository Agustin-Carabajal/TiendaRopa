using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos;

namespace TiendaRopa.Repositorio.Repositorios
{
    public class Repositorio<E> : IRepositorio<E> where E : class, IEntityBase
    {
        private readonly AppDbContext context;

        public Repositorio(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Existe(int id)
        {
            bool existe = await context.Set<E>().AnyAsync(x => x.Id == id);
            return existe;
        }
        public async Task<List<E>> Select()
        {
            return await context.Set<E>().ToListAsync();
        }

        public async Task<E?> SelectById(int id)
        {
            return await context.Set<E>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<int> Insert(E entity)
        {
            try
            {
                context.Set<E>().Add(entity);
                await context.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception err) { throw err; }

        }

        public async Task<bool> Update(int id, E entity)
        {
            if (id != entity.Id)
            {
                return false;
            }

            var existe = await Existe(id);
            if (!existe)
            {
                return false;
            }

            try
            {
                context.Set<E>().Update(entity);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception err) { throw err; }
        }

        public async Task<bool> Delete(int id)
        {
            var entidad = await SelectById(id);
            if (entidad == null)
            {
                return false;
            }
            try
            {
                context.Set<E>().Remove(entidad);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception err) { throw err; }
        }
    }
}
