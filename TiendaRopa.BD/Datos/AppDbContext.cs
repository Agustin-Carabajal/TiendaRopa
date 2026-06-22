using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos.Entity;

namespace TiendaRopa.BD.Datos
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Talle> Talles { get; set; }
        public DbSet<Color> Colores { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuración adicional del modelo si es necesario
            modelBuilder.Entity<ApplicationUser>()
                .HasKey(u => u.Id); // Configura la clave primaria para ApplicationUser)

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => ((IdentityUser)u).Id)
                .HasColumnName("IdentityId"); // Configura el nombre de la columna para la propiedad Id

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }
}
