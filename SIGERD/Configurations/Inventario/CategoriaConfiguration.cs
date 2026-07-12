using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Inventario; 

namespace SIGERD.Configurations.Inventario
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> entity)
        {
            entity.ToTable("Categorias", "Inventario");
            entity.HasKey(c => c.idCategoria);
            entity.Property(c => c.nombreCategoria)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
