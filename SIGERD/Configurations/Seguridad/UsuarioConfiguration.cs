using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Seguridad;

namespace SIGERD.Configurations.Seguridad
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> entity)
        {
            entity.ToTable("Usuarios", "Seguridad");

            entity.HasKey(u => u.idUsuario);

            entity.Property(u => u.nombreCompleto)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(u => u.correo)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(u => u.clave)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.idRolUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(u => u.estado)
                .IsRequired();
        }
    }
}
