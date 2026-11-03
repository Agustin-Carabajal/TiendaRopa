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

        // Credenciales del admin: salen de la configuración, no del código
        var email = config["AdminSeed:Email"];
        var password = config["AdminSeed:Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;

        var admin = await userManager.FindByEmailAsync(email);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,              // necesario si RequireConfirmedAccount = true
                EstadoRegistro = EstadoRegistro.activo // ajustá al nombre real de tu enum
            };

            var result = await userManager.CreateAsync(admin, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    "No se pudo crear el admin: " + string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        if (!await userManager.IsInRoleAsync(admin, "Admin"))
            await userManager.AddToRoleAsync(admin, "Admin");
    }
}
