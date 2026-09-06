using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TiendaRopa.BD.Datos.Entity;

namespace TiendaRopa.Datos.Configurations;

public class CompradorConfiguration : IEntityTypeConfiguration<Comprador>
{
    public void Configure(EntityTypeBuilder<Comprador> builder)
    {
        builder.ToTable("Compradores", t => t.HasCheckConstraint(
            "CK_Comprador_OrigenUnico",
            "(\"ClienteId\" IS NOT NULL AND \"ApplicationUserId\" IS NULL) " +
            "OR (\"ClienteId\" IS NULL AND \"ApplicationUserId\" IS NOT NULL)"));

        builder.Property(c => c.Origen)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(c => c.Cliente)
            .WithMany()
            .HasForeignKey(c => c.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.ApplicationUser)
            .WithMany()
            .HasForeignKey(c => c.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Un mismo Cliente o ApplicationUser no debería tener más de un Comprador asociado.
        builder.HasIndex(c => c.ClienteId).IsUnique().HasFilter("\"ClienteId\" IS NOT NULL");
        builder.HasIndex(c => c.ApplicationUserId).IsUnique().HasFilter("\"ApplicationUserId\" IS NOT NULL");
    }
}