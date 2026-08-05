using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Envios;

namespace SIGERD.Configurations.Envios
{
    public class DetalleEnvioConfiguration : IEntityTypeConfiguration<DetalleEnvio>
    {
        public void Configure(EntityTypeBuilder<DetalleEnvio> entity)
        {
            entity.ToTable("DetalleEnvio", "Envios");

            entity.HasKey(d => d.idDetalle);

            entity.Property(d => d.idDetalle)
                .HasColumnName("idDetalle");

            entity.Property(d => d.idEnvioDetalleEnvio)
                .HasColumnName("idEnvioDetalleEnvio");

            entity.Property(d => d.idArticuloDetalleEnvio)
                .HasColumnName("idArticuloDetalleEnvio");

            entity.Property(d => d.cantidad)
                .HasColumnName("cantidad")
                .IsRequired();

            entity.Property(d => d.observacionesDetalleEnvio)
                .HasColumnName("observacionesDetalleEnvio")
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Envio)
                .WithMany(e => e.DetallesEnvio)
                .HasForeignKey(d => d.idEnvioDetalleEnvio)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Articulo)
                .WithMany(a => a.DetallesEnvio)
                .HasForeignKey(d => d.idArticuloDetalleEnvio)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
