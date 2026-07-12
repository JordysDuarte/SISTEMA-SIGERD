using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Seguridad;

namespace SIGERD.Configurations.Seguridad
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> entity)
        {
            entity.ToTable("Roles", "Seguridad");

            entity.HasKey(r => r.idRol);

            entity.Property(r => r.nombreRol)
               .HasMaxLength(50)
               .IsRequired();

            entity.Property(r => r.descripcion)
               .HasMaxLength(150);
        }

    }
}
