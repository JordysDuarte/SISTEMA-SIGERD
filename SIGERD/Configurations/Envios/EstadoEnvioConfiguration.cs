using Microsoft.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Envios;

namespace SIGERD.Configurations.Envios
{
    public class EstadoEnvioConfiguration : IEntityTypeConfiguration<EstadoEnvio>
    {
        public void Configure(EntityTypeBuilder<EstadoEnvio> builder)
        {
            builder.ToTable("EstadoEnvios", "Envios");
            builder.HasKey(e => e.idEstadoEnvio);
            builder.Property(e => e.nombreEstadoEnvio)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(e => e.descripcion)
                .HasMaxLength(200);
        }
    }
}
