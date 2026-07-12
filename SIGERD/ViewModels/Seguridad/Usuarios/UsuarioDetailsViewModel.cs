using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Seguridad.Usuarios
{
    public class UsuarioDetailsViewModel
    {
        [Display(Name = "Código")]   
        public int IdUsuario { get; set; }

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; } = string.Empty;


        [Display(Name = "Rol")]
        public string Rol { get; set; } = string.Empty;

        [Display(Name = "Delegación")]
        public string Delegacion {  get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public bool Estado { get; set; }

        [Display(Name = "Debe cambiar contraseña")]
        public bool DebeCambiarClave { get; set; }
    }
}
