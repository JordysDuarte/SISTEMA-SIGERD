using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Inventario;

namespace SIGERD.Configurations.Inventario
{
    public class ArticuloConfiguration : IEntityTypeConfiguration<Articulo>
    {
        public void Configure(EntityTypeBuilder<Articulo> builder)
        {
            builder.ToTable("Articulos", "Inventario");

            builder.HasKey(a => a.idArticulo);

            builder.Property(a => a.idArticulo)
                .HasColumnName("idArticulo");

            builder.Property(a => a.nombreArticulo)
                .HasColumnName("nombreArticulo")
                .HasMaxLength(150)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(a => a.descripcion)
                .HasColumnName("descripcion")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(a => a.estado)
                .HasColumnName("estado");

            builder.Property(a => a.idCategoriaArticulo)
                .HasColumnName("idCategoriaArticulo");

            builder.HasOne(a => a.Categoria)
                .WithMany(c => c.Articulos)
                .HasForeignKey(a => a.idCategoriaArticulo)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.nombreArticulo)
                .IsUnique();
        }
    }
}