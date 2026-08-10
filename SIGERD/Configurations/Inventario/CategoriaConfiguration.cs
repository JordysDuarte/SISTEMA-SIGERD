using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Inventario;

namespace SIGERD.Configurations.Inventario
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias", "Inventario");

            builder.HasKey(c => c.idCategoria);

            builder.Property(c => c.idCategoria)
                .HasColumnName("idCategoria");

            builder.Property(c => c.nombreCategoria)
                .HasColumnName("nombreCategoria")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(c => c.estado)
                .HasColumnName("estado");

            builder.HasIndex(c => c.nombreCategoria)
                .IsUnique();
        }
    }
}