using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Seguridad.Usuarios
{
    public class UsuarioCreateViewModel
    {
        [Display(Name = "Nombre Completo")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 carácteres.")]
        public string NombreCompleto { get; set; } = string.Empty;


        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        public string Correo { get; set; } = string.Empty;


        [Display(Name = "Constraseña")]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 carácteres.")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = string.Empty;

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        public int IdRol {  get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
