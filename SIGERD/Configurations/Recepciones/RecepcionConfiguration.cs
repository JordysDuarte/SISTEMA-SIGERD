using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Recepciones;

namespace SIGERD.Configurations.Recepciones
{
    public class RecepcionConfiguration : IEntityTypeConfiguration<Recepcion>
    {
        public void Configure(EntityTypeBuilder<Recepcion> builder)
        {
            builder.ToTable("Recepciones", "Recepciones");

            builder.HasKey(r => r.idRecepcion);

            builder.Property(r => r.idRecepcion)
                .HasColumnName("idRecepcion");

            builder.Property(r => r.fechaRecepcion)
                .HasColumnName("fechaRecepcion")
                .HasColumnType("datetime2");

            builder.Property(r => r.idEnvioRecepcion)
                .HasColumnName("idEnvioRecepcion");

            builder.Property(r => r.idUsuarioRecepcion)
                .HasColumnName("idUsuarioRecepcion");

            builder.Property(r => r.observaciones)
                .HasColumnName("observaciones")
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.HasIndex(r => r.idEnvioRecepcion)
                .IsUnique();

            builder.HasOne(r => r.Envio)
                .WithOne(e => e.Recepcion)
                .HasForeignKey<Recepcion>(r => r.idEnvioRecepcion)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Usuario)
                .WithMany(u => u.Recepciones)
                .HasForeignKey(r => r.idUsuarioRecepcion)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}