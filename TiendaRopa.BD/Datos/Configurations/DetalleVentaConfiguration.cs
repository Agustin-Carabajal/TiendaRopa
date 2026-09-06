using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TiendaRopa.BD.Datos.Entity;


namespace TiendaRopa.BD.Datos.Configurations;

public class DetalleVentaConfiguration : IEntityTypeConfiguration<DetalleVenta>
{
    public void Configure(EntityTypeBuilder<DetalleVenta> builder)
    {
        builder.ToTable("DetallesVenta");

        builder.Property(d => d.PrecioUnitario).HasColumnType("decimal(18,2)");
        builder.Property(d => d.Subtotal).HasColumnType("decimal(18,2)");

        builder.HasOne(d => d.Venta)
            .WithMany(v => v.DetallesVenta)
            .HasForeignKey(d => d.VentaId)
            .OnDelete(DeleteBehavior.Restrict); // consistente con la regla global: nunca cascada en registros financieros

        builder.HasOne(d => d.Variante)
            .WithMany()
            .HasForeignKey(d => d.VarianteId)
            .OnDelete(DeleteBehavior.Restrict); // no borrar una Variante que aparece en ventas históricas
    }
}
