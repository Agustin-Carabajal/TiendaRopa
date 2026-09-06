using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TiendaRopa.BD.Datos.Entity;

namespace TiendaRopa.Datos.Configurations;

public class VentaConfiguration : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        builder.ToTable("Ventas");

        builder.Property(v => v.Monto).HasColumnType("decimal(18,2)");
        builder.Property(v => v.Estado).HasMaxLength(30).IsRequired();
        builder.Property(v => v.CanalDeVenta)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(v => v.Comprador)
            .WithMany(c => c.Ventas)
            .HasForeignKey(v => v.CompradorId)
            .OnDelete(DeleteBehavior.Restrict);

        // 1 a 1 opcional: solo las ventas web nacen de un carrito.
        builder.HasOne(v => v.Carrito)
            .WithOne(c => c.Venta)
            .HasForeignKey<Venta>(v => v.CarritoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
