using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos;

namespace TiendaRopa.Servicio
{
    public class UsuarioAdminService(UserManager<ApplicationUser> userManager)
    {
        public async Task<(bool Ok, string? Error)> HacerAdminAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return (false, "No existe una cuenta con ese email.");
            if (!user.EmailConfirmed) return (false, "Esa cuenta todavía no confirmó su email.");
            if (await userManager.IsInRoleAsync(user, "Admin")) return (true, null);

            var r = await userManager.AddToRoleAsync(user, "Admin");
            if (!r.Succeeded) return (false, string.Join("; ", r.Errors.Select(e => e.Description)));

            await userManager.UpdateSecurityStampAsync(user); // obliga a refrescar sus roles
            return (true, null);
        }

        public async Task<(bool Ok, string? Error)> QuitarAdminAsync(string userId)
        {
            var admins = await userManager.GetUsersInRoleAsync("Admin");
            if (admins.Count <= 1) return (false, "Tiene que quedar al menos un administrador.");

            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return (false, "Usuario no encontrado.");

            var r = await userManager.RemoveFromRoleAsync(user, "Admin");
            if (!r.Succeeded) return (false, string.Join("; ", r.Errors.Select(e => e.Description)));

            await userManager.UpdateSecurityStampAsync(user);
            return (true, null);
        }
    }
}
