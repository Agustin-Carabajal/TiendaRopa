using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Cliente;


namespace TiendaRopa.Repositorio.Repositorios.ClientesRep
{
    public class ClienteRepositorio : Repositorio<Cliente>, IRepositorio<Cliente>, IClienteRepositorio
    {
        private readonly AppDbContext context;

        public ClienteRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<ClienteMostrarDTO>> SelectListaClientes()
        {
            var lista = await context.Clientes.Select(x => new ClienteMostrarDTO
            {
                Id = x.Id,
                Nombre = x.Nombre,
                Apellido = x.Apellido,
                Dni = x.Dni,
                FechaNacimiento = x.FechaNacimiento,
                Domicilio = x.Domicilio,
                Telefono = x.Telefono,
                Saldo = x.Saldo,
                Rol = x.Rol
            }).ToListAsync();
            return lista;
        }

        public async Task<bool> ExisteNombre(string nombre, string apellido)
        {
            // Busca en la base de datos si hay coincidencia exacta sin importar mayúsculas.
            // Normalizar parámetros fuera del árbol de expresión para evitar NullReferenceException.
            var nombreNorm = (nombre ?? string.Empty).ToLower().Trim();
            var apellidoNorm = (apellido ?? string.Empty).ToLower().Trim();

            return await context.Set<Cliente>()
                .AnyAsync(m => ((m.Nombre ?? string.Empty).ToLower().Trim() == nombreNorm) && ((m.Apellido ?? string.Empty).ToLower().Trim() == apellidoNorm));
        }
        //explanation":"Normalizar parámetros nulos y evitar la evaluación de ToLower/Trim sobre parámetros nulos en el árbol de expresión; usar coalesce en propiedades de entidad para evitar NullReferenceException."}```

        public async Task<ClienteMostrarDTO?> ObtenerById(int id)
        {
            return await context.Clientes
                .Where(c => c.Id == id)
                .Select(c => new ClienteMostrarDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Dni = c.Dni,
                    FechaNacimiento = c.FechaNacimiento,
                    Domicilio = c.Domicilio,
                    Telefono = c.Telefono,
                    Saldo = c.Saldo,
                    Rol = c.Rol
                })
                .FirstOrDefaultAsync();
        }

    }
}
