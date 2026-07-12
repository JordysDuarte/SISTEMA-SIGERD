using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Seguridad.Usuarios
{
    public class UsuarioCreateViewModel
    {
        [Display(Name = "Nombre completo")]
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre completo no debe superar los 100 caracteres.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "El nombre de usuario debe tener entre 4 y 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "El nombre de usuario solo puede contener letras, números, punto, guion y guion bajo.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        [StringLength(150, ErrorMessage = "El correo no debe superar los 150 caracteres.")]
        public string Correo { get; set; } = string.Empty;

        [Display(Name = "Contraseña inicial")]
        [Required(ErrorMessage = "La contraseña inicial es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Clave { get; set; } = string.Empty;

        [Display(Name = "Confirmar contraseña")]
        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Clave), ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarClave { get; set; } = string.Empty;

        [Display(Name = "Rol")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol.")]
        public int IdRol { get; set; }

        [Display(Name = "Delegación")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una delegación.")]
        public int IdDelegacion { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Delegaciones { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}