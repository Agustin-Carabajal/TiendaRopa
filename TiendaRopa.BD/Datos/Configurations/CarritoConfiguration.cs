using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TiendaRopa.BD.Datos.Entity;

namespace TiendaRopa.Datos.Configurations;

public class CarritoConfiguration : IEntityTypeConfiguration<Carrito>
{
    public void Configure(EntityTypeBuilder<Carrito> builder)
    {
        builder.ToTable("Carritos");

        builder.Property(c => c.Monto).HasColumnType("decimal(18,2)");
        builder.Property(c => c.Estado).HasMaxLength(30).IsRequired();

        builder.HasOne(c => c.Comprador)
            .WithMany(cp => cp.Carritos)
            .HasForeignKey(c => c.CompradorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
