using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Ubicacion;

namespace SIGERD.Configurations.Ubicacion
{
    public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
    {
        public void Configure(EntityTypeBuilder<Departamento> entity)
        {
            entity.ToTable("Departamentos", "Ubicacion");
            entity.HasKey(d => d.idDepartamento);
            entity.Property(d => d.nombreDepartamento)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
