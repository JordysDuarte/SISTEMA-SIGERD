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

            entity.HasOne(e => e.DelegacionOrigen)
                .WithMany(d => d.EnviosOrigen)
                .HasForeignKey(e => e.idDelegacionOrigenEnvio)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DelegacionDestino)
                .WithMany(d => d.EnviosDestino)
                .HasForeignKey(e => e.idDelegacionDestinoEnvio)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Envios)
                .HasForeignKey(e => e.idUsuarioEnvio)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.fechaDespacho)
                .HasColumnName("fechaDespacho")
                .HasColumnType("datetime2");

            entity.Property(e => e.idUsuarioDespacho)
                .HasColumnName("idUsuarioDespacho");

            entity.HasOne(e => e.UsuarioDespacho)
                .WithMany()
                .HasForeignKey(e => e.idUsuarioDespacho)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
