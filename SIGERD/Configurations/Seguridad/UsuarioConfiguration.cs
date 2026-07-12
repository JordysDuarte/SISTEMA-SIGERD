using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Seguridad;

namespace SIGERD.Configurations.Seguridad
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios", "Seguridad");

            builder.HasKey(u => u.idUsuario);

            builder.Property(u => u.idUsuario)
                .HasColumnName("idUsuario");

            builder.Property(u => u.nombreCompleto)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(u => u.nombreUsuario)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(u => u.correo)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("varchar(150)");

            builder.Property(u => u.claveHash)
                .IsRequired()
                .HasMaxLength(512)
                .HasColumnType("varchar(512)");

            builder.Property(u => u.estado)
                .IsRequired();

            builder.Property(u => u.debeCambiarClave)
                .IsRequired();

            builder.Property(u => u.fechaUltimoCambioClave)
                .HasColumnType("datetime2");

            builder.Property(u => u.VersionSeguridad)
                .IsRequired();

            builder.Property(u => u.idRolUsuario)
                .IsRequired();

            builder.Property(u => u.idDelegacionUsuario)
                .IsRequired();

            builder.HasIndex(u => u.nombreUsuario)
                .IsUnique()
                .HasDatabaseName("UX_Usuarios_NombreUsuario");

            builder.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.idRolUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Delegacion)
                .WithMany()
                .HasForeignKey(u => u.idDelegacionUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}