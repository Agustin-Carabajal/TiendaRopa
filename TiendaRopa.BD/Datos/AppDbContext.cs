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
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Recepcion> Recepciones { get; set; }
        public DbSet<DetallesRecepcion> DetallesRecepciones { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 2. Renombrar la columna Id a IdentityId (si aún deseas mantener este nombre en la base de datos)
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.Id)
                .HasColumnName("IdentityId");

            // 3. Configurar la precisión de los decimales para el Saldo del usuario
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.Saldo)
                .HasPrecision(18, 2);

            // 4. Desactivar el borrado en cascada global para evitar bloqueos en SQL Server
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
