using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Seguridad.Usuarios
{
    public class UsuarioEditViewModel
    {
        public int IdUsuario { get; set; }


        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre completo no debe superar los 100 carácteres.")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no debe superar los 50 carácteres.")]
        [MinLength(4, ErrorMessage = "El nombre de usuario debe tener al menos 4 carácteres.")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "El nombre de usuario solo puede contener letras, números, puntos, guiones y guiones bajos")]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        [StringLength(150, ErrorMessage = "El correo electrónico no debe superar los 150 carácteres")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol.")]
        [Display(Name = "Rol")]
        public int IdRol {  get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una delegación.")]
        [Display(Name = "Delegación")]
        public int IdDelegacion { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Delegaciones { get; set;} = new List<SelectListItem>(); 
    }
}
