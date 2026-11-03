using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TiendaRopa.BD.Datos.Entity;
namespace TiendaRopa.BD.Datos.Configurations;
public class PagoVentaConfiguration : IEntityTypeConfiguration<PagoVenta> { public void Configure(EntityTypeBuilder<PagoVenta> builder) { builder.ToTable("PagosVenta"); builder.Property(p=>p.Monto).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired(); } }
