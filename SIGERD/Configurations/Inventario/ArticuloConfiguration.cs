using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGERD.Models.Inventario;

namespace SIGERD.Configurations.Inventario
{
    public class ArticuloConfiguration : IEntityTypeConfiguration<Articulo>
    {
        public void Configure(EntityTypeBuilder<Articulo> entity)
        {
            entity.ToTable("Articulos", "Inventario");
            entity.HasKey(a => a.idArticulo);

            entity.Property(a => a.nombreArticulo)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(a => a.descripcion)
                .HasMaxLength(200);

            entity.HasOne(a => a.Categoria)
                  .WithMany(c => c.Articulos)
                  .HasForeignKey(a => a.idCategoriaArticulo)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
