using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Inventario.Articulos
{
    public class ArticuloCreateViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el nombre del artículo.")]
        [StringLength(150, ErrorMessage = "El nombre del artículo no debe superar los 150 caracteres.")]
        [Display(Name = "Nombre del artículo")]
        public string NombreArticulo { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La descripción no debe superar los 200 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        [Display(Name = "Categoría")]
        public int IdCategoriaArticulo { get; set; }

        public IEnumerable<SelectListItem> Categorias { get; set; } = new List<SelectListItem>();
    }
}