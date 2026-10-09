using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.BD.Datos.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes", t =>
            {
                // El fiado solo aplica a clientes presenciales
                t.HasCheckConstraint(
                    "CK_Clientes_Saldo_Solo_Presencial",
                    "(\"Origen\" = 'Presencial') OR (\"Saldo\" = 0)");

                // Web <=> tiene usuario; Presencial <=> no tiene usuario
                t.HasCheckConstraint(
                    "CK_Clientes_Origen_Usuario",
                    "(\"Origen\" = 'Web' AND \"ApplicationUserId\" IS NOT NULL) " +
                    "OR (\"Origen\" = 'Presencial' AND \"ApplicationUserId\" IS NULL)");
            });

            // Datos personales
            builder.Property(c => c.Nombre)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Apellido)
                   .HasMaxLength(100);

            builder.Property(c => c.Dni)
                   .HasMaxLength(8);

            builder.Property(c => c.Domicilio)
                   .HasMaxLength(300);

            builder.Property(c => c.Telefono)
                   .HasMaxLength(20);

            builder.Property(c => c.FechaNacimiento)
                   .HasColumnType("date");

            // Fiado
            builder.Property(c => c.Saldo)
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0m);

            // Origen guardado como texto legible
            builder.Property(c => c.Origen)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .HasDefaultValue(OrigenCliente.Presencial);

            // Relación 1 a 1 opcional con Identity
            builder.HasOne(c => c.ApplicationUser)
                   .WithOne(u => u.Cliente)
                   .HasForeignKey<Cliente>(c => c.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Índices
            // Búsqueda por nombre (NO único: puede haber homónimos)
            builder.HasIndex(c => new { c.Apellido, c.Nombre })
                   .HasDatabaseName("IX_Clientes_Apellido_Nombre");

            // DNI único solo entre presenciales y solo si está cargado
            builder.HasIndex(c => c.Dni)
                   .IsUnique()
                   .HasDatabaseName("UX_Clientes_Dni_Presencial")
                   .HasFilter("\"Dni\" IS NOT NULL AND \"Origen\" = 'Presencial'");

            // Un usuario web, un solo cliente (PostgreSQL permite varios NULL)
            builder.HasIndex(c => c.ApplicationUserId)
                   .IsUnique()
                   .HasDatabaseName("UX_Clientes_ApplicationUserId");
        }
    }
}
