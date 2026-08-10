using SIGERD.Models.Inventario;
using SIGERD.ViewModels.Inventario.Articulos;

namespace SIGERD.Mappings
{
    public static class ArticuloMapper
    {
        public static ArticuloListViewModel ToListViewModel(Articulo articulo)
        {
            return new ArticuloListViewModel
            {
                IdArticulo = articulo.idArticulo,
                NombreArticulo = articulo.nombreArticulo,
                Descripcion = articulo.descripcion,
                Categoria = articulo.Categoria?.nombreCategoria ?? "Sin categoría",
                Estado = articulo.estado
            };
        }

        public static ArticuloDetailsViewModel ToDetailsViewModel(Articulo articulo)
        {
            return new ArticuloDetailsViewModel
            {
                IdArticulo = articulo.idArticulo,
                NombreArticulo = articulo.nombreArticulo,
                Descripcion = articulo.descripcion,
                Categoria = articulo.Categoria?.nombreCategoria ?? "Sin categoría",
                Estado = articulo.estado
            };
        }

        public static ArticuloEditViewModel ToEditViewModel(Articulo articulo)
        {
            return new ArticuloEditViewModel
            {
                IdArticulo = articulo.idArticulo,
                NombreArticulo = articulo.nombreArticulo,
                Descripcion = articulo.descripcion,
                IdCategoriaArticulo = articulo.idCategoriaArticulo,
                Estado = articulo.estado
            };
        }

        public static Articulo ToEntity(ArticuloCreateViewModel model)
        {
            return new Articulo
            {
                nombreArticulo = model.NombreArticulo.Trim(),
                descripcion = model.Descripcion?.Trim(),
                idCategoriaArticulo = model.IdCategoriaArticulo,
                estado = true
            };
        }

        public static Articulo ToEntity(ArticuloEditViewModel model)
        {
            return new Articulo
            {
                idArticulo = model.IdArticulo,
                nombreArticulo = model.NombreArticulo.Trim(),
                descripcion = model.Descripcion?.Trim(),
                idCategoriaArticulo = model.IdCategoriaArticulo,
                estado = model.Estado
            };
        }
    }
}