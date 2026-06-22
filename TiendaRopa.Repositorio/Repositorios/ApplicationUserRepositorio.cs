using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos;
using TiendaRopa.Shared.DTO;

namespace TiendaRopa.Repositorio.Repositorios
{
    public class ApplicationUserRepositorio : Repositorio<ApplicationUser>, IRepositorio<ApplicationUser>, IApplicationUserRepositorio
    {
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ApplicationUserRepositorio(AppDbContext context,
                                            UserManager<ApplicationUser> userManager,
                                            RoleManager<IdentityRole> roleManager) : base(context)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<List<UsuarioMostrarDTO>> SelectListaUsuarios()
        {
            var lista = await context.Set<ApplicationUser>().Select(x => new UsuarioMostrarDTO
            {
                Id = ((Microsoft.AspNetCore.Identity.IdentityUser)x).Id,
                IntId = x.Id,
                NombreUsuario = x.NombreUsuario,
                ApellidoUsuario = x.ApellidoUsuario,
                DniUsuario = x.DniUsuario,
                SaldoUsuario = x.SaldoUsuario
            }).ToListAsync();
            return lista;
        }

        public async Task<bool> RegistrarUsuarioSeguro(UsuarioCrearDTO model)
        {
            var nuevoUsuario = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NombreUsuario = model.NombreUsuario,
                ApellidoUsuario = model.ApellidoUsuario,
                DniUsuario = model.DniUsuario,
                FechaNacimientoUsuario = model.FechaNacimientoUsuario,
                DireccionUsuario = model.DireccionUsuario,
                SaldoUsuario = model.SaldoUsuario,
                EmailConfirmed = true
            };
            var resultado = await userManager.CreateAsync(nuevoUsuario, model.Password);
            if (resultado.Succeeded)
            {
                // Si quieres asignar un rol específico al usuario, hazlo aquí
                if (!string.IsNullOrEmpty(model.Rol))
                {
                    var rolExiste = await roleManager.RoleExistsAsync(model.Rol);
                    if (rolExiste)
                    {
                        await userManager.AddToRoleAsync(nuevoUsuario, model.Rol);
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(nuevoUsuario, "Cliente");
                    }
                }
                return true;
                
            }
            return false;

        }
    }
}
