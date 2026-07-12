using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Recepciones;

namespace SIGERD.Configurations.Recepciones
{
    public class RecepcionConfiguration : IEntityTypeConfiguration<Recepcion>
    {
        public void Configure(EntityTypeBuilder<Recepcion> entity)
        {
            entity.ToTable("Recepciones", "Recepciones");

            entity.HasKey(r => r.idRecepcion);

            entity.Property(r => r.fechaRecepcion)
                .IsRequired();

            entity.Property(r => r.observaciones)
                .HasMaxLength(300);

            entity.HasOne(r => r.Envio)
                .WithOne(e => e.Recepcion)
                .HasForeignKey<Recepcion>(r => r.idEnvioRecepcion)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Usuario)
                .WithMany(u => u.Recepciones)
                .HasForeignKey(r => r.idUsuarioRecepcion)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
