using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Shared.ENUM;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        foreach (var tipo in new[] { "Efectivo", "Débito", "Crédito", "QR", "Transferencia Bancaria" })
            if (!db.Pagos.Any(p => p.Tipo == tipo))
                db.Pagos.Add(new Pago { Tipo = tipo, EstadoRegistro = EstadoRegistro.activo });
        await db.SaveChangesAsync();

        // Roles: se crean si no existen (también los necesita el registro web)
        foreach (var rol in new[] { "Admin", "Cliente" })
        {
            if (!await roleManager.RoleExistsAsync(rol))
                await roleManager.CreateAsync(new IdentityRole(rol));
        }

        // Las credenciales salen de User Secrets o de otra fuente de configuración,
        // no quedan guardadas en el código ni en appsettings.
        foreach (var adminConfigKey in new[] { "AdminSeed", "AdminSeed2" })
        {
            var email = config[$"{adminConfigKey}:Email"];
            var password = config[$"{adminConfigKey}:Password"];
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                continue;

            var admin = await userManager.FindByEmailAsync(email);
            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true, // necesario si RequireConfirmedAccount = true
                    EstadoRegistro = EstadoRegistro.activo
                };

                var result = await userManager.CreateAsync(admin, password);
                if (!result.Succeeded)
                    throw new InvalidOperationException(
                        $"No se pudo crear el admin configurado en {adminConfigKey}: " +
                        string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                var roleResult = await userManager.AddToRoleAsync(admin, "Admin");
                if (!roleResult.Succeeded)
                    throw new InvalidOperationException(
                        $"No se pudo asignar el rol Admin a {email}: " +
                        string.Join("; ", roleResult.Errors.Select(e => e.Description)));
            }
        }
    }
}
