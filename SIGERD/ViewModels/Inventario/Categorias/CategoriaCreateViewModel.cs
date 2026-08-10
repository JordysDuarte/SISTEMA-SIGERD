using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Inventario.Categorias
{
    public class CategoriaCreateViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el nombre de la categoría.")]
        [StringLength(100, ErrorMessage = "El nombre de la categoría no debe superar los 100 caracteres.")]
        [Display(Name = "Nombre de la categoría")]
        public string NombreCategoria { get; set; } = string.Empty;
    }
}