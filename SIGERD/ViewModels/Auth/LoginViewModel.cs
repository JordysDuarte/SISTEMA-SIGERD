using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "Debe ingresar su nombre de usuario.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Debe ingresar su constraseña.")]
        public string Clave { get; set; } = string.Empty;

        [Display(Name = "Recordarme")]
        public bool Recordarme { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
