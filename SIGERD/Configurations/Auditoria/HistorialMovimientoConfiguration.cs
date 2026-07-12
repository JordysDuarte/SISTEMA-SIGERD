using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Auditoria;

namespace SIGERD.Configurations.Auditoria
{
    public class HistorialMovimientoConfiguration : IEntityTypeConfiguration<HistorialMovimiento>
    {
        public void Configure(EntityTypeBuilder<HistorialMovimiento> entity)
        {
            entity.ToTable("HistorialMovimientos", "Auditoria");

            entity.HasKey(h => h.idHistorialMovimiento);

            entity.Property(h => h.fechaMovimiento)
                .IsRequired();

            entity.Property(h => h.observaciones)
                .HasMaxLength(300);

            entity.HasOne(h => h.Envio)
                .WithMany(e => e.HistorialMovimientos)
                .HasForeignKey(h => h.idEnvioHistorialMovimiento)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(h => h.TipoMovimiento)
                .WithMany(t => t.HistorialMovimientos)
                .HasForeignKey(h => h.idTipoMovimientoHistorialMovimiento)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(h => h.EstadoEnvio)
                .WithMany(e => e.HistorialMovimientos)
                .HasForeignKey(h => h.idEstadoEnvioHistorialMovimiento)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(h => h.Usuario)
                .WithMany(u => u.HistorialMovimientos)
                .HasForeignKey(h => h.idUsuarioHistorialMovimiento)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
