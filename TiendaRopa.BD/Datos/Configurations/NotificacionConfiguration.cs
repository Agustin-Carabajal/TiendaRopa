using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TiendaRopa.BD.Datos.Entity;

namespace TiendaRopa.Datos.Configurations;

public class NotificacionConfiguration : IEntityTypeConfiguration<Notificacion>
{
    public void Configure(EntityTypeBuilder<Notificacion> builder)
    {
        builder.ToTable("Notificaciones");

        builder.Property(n => n.Mensaje).HasMaxLength(300).IsRequired();
        builder.Property(n => n.Tipo)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();
        builder.Property(n => n.TipoEntidad)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(n => n.EntidadRelacionadaId).HasMaxLength(450).IsRequired();

        // Consulta típica: "notificaciones no leídas de tal entidad".
        builder.HasIndex(n => new { n.TipoEntidad, n.EntidadRelacionadaId, n.Leida });
    }
}
