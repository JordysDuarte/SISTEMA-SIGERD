using SIGERD.Models.Inventario;
using SIGERD.ViewModels.Inventario.Categorias;

namespace SIGERD.Mappings
{
    public static class CategoriaMapper
    {
        public static CategoriaListViewModel ToListViewModel(Categoria categoria)
        {
            return new CategoriaListViewModel
            {
                IdCategoria = categoria.idCategoria,
                NombreCategoria = categoria.nombreCategoria,
                Estado = categoria.estado,
                TotalArticulos = categoria.Articulos?.Count ?? 0
            };
        }

        public static CategoriaDetailsViewModel ToDetailsViewModel(Categoria categoria)
        {
            return new CategoriaDetailsViewModel
            {
                IdCategoria = categoria.idCategoria,
                NombreCategoria = categoria.nombreCategoria,
                Estado = categoria.estado,
                TotalArticulos = categoria.Articulos?.Count ?? 0
            };
        }

        public static CategoriaEditViewModel ToEditViewModel(Categoria categoria)
        {
            return new CategoriaEditViewModel
            {
                IdCategoria = categoria.idCategoria,
                NombreCategoria = categoria.nombreCategoria,
                Estado = categoria.estado
            };
        }

        public static Categoria ToEntity(CategoriaCreateViewModel model)
        {
            return new Categoria
            {
                nombreCategoria = model.NombreCategoria.Trim(),
                estado = true
            };
        }

        public static Categoria ToEntity(CategoriaEditViewModel model)
        {
            return new Categoria
            {
                idCategoria = model.IdCategoria,
                nombreCategoria = model.NombreCategoria.Trim(),
                estado = model.Estado
            };
        }
    }
}