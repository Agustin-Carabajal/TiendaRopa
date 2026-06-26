using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Usuario;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Repositorio.Repositorios.Usuario
{
    public class ApplicationUserRepositorio :  IApplicationUserRepositorio
    {
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ApplicationUserRepositorio(AppDbContext context,
                                            UserManager<ApplicationUser> userManager,
                                            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<List<UsuarioMostrarDTO>> SelectListaUsuarios()
        {
            var lista = await context.Set<ApplicationUser>().Select(x => new UsuarioMostrarDTO
            {
                Id = x.Id,
                Nombre = x.Nombre,
                Apellido = x.Apellido,
                Dni = x.Dni,
                Saldo = x.Saldo
            }).ToListAsync();
            return lista;
        }

        public async Task<IdentityResult> RegistrarUsuarioSeguro(UsuarioCrearDTO model)
        {
            var nuevoUsuario = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Dni = model.Dni,
                FechaNacimiento = model.FechaNacimiento,
                Direccion = model.Direccion,
                Saldo = model.Saldo,
                EstadoRegistro = EstadoRegistro.activo
            };
            var resultado = await userManager.CreateAsync(nuevoUsuario, model.Password);
            if (!resultado.Succeeded)
            {
                return resultado;
            }

            if (!await roleManager.RoleExistsAsync(model.Rol)) 
            {
                await roleManager.CreateAsync(new IdentityRole(model.Rol));
            }
            return await userManager.AddToRoleAsync(nuevoUsuario, model.Rol);
        }
             
        public async Task<IdentityResult> UpdateUsuario(ApplicationUser usuario)
        {
            return await userManager.UpdateAsync(usuario);
        }

        

        public async Task<UsuarioMostrarDTO?> ObtenerPorIdAsync(string id)
        {
            return await context.Users
                .Where(x => x.Id == id)
                .Select(x => new UsuarioMostrarDTO
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Apellido = x.Apellido,
                    Dni = x.Dni,
                    Saldo = x.Saldo
                }).FirstOrDefaultAsync();
        }
        public async Task<ApplicationUser?> ObtenerEntidadStringId(string id)
        {
            return await userManager.FindByIdAsync(id);

        }


    }
}
