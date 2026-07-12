using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Auditoria;

namespace SIGERD.Configurations.Auditoria
{
    public class TipoMovimientoConfiguration : IEntityTypeConfiguration<TipoMovimiento>
    {
        public void Configure(EntityTypeBuilder<TipoMovimiento> entity)
        {
            entity.ToTable("TiposMovimiento", "Auditoria");

            entity.HasKey(t => t.idTipoMovimiento);

            entity.Property(t => t.nombreMovimiento)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(t => t.descripcion)
                .HasMaxLength(200);
        }
    }
}
