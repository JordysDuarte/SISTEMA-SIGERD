using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Envios;

namespace SIGERD.Configurations.Envios
{
    public class EnvioConfiguration : IEntityTypeConfiguration<Envio>
    {
        public void Configure(EntityTypeBuilder<Envio> entity)
        {
            entity.ToTable("Envios", "Envios");

            entity.HasKey(e => e.idEnvio);

            entity.Property(e => e.codigoEnvio)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.fechaEnvio)
                .IsRequired();

            entity.Property(e => e.observaciones)
                .HasMaxLength(300);

            entity.HasOne(e => e.EstadoEnvio)
                .WithMany(est => est.Envios)
                .HasForeignKey(e => e.idEstadoEnvioEnvio)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Delegacion)
                .WithMany(d => d.Envios)
                .HasForeignKey(e => e.idDelegacionEnvio)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Envios)
                .HasForeignKey(e => e.idUsuarioEnvio)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
