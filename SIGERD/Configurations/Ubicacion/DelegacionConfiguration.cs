using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Ubicacion;

namespace SIGERD.Configurations.Ubicacion
{
    public class DelegacionConfiguration : IEntityTypeConfiguration<Delegacion>
    {
        public void Configure(EntityTypeBuilder<Delegacion> entity)
        {
            entity.ToTable("Delegaciones", "Ubicacion");

            entity.HasKey(d => d.idDelegacion);

            entity.Property(d => d.nombreDelegacion)
                .HasMaxLength(150)
                .IsRequired();
            entity.Property(d => d.direccion)
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(d => d.telefono)
                .HasMaxLength(20);
            entity.HasOne(d => d.Departamento)
                .WithMany(dep => dep.Delegaciones)
                .HasForeignKey(d => d.idDepartamentoDelegacion)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
