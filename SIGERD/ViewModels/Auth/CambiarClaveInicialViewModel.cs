using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Auth
{
    public class CambiarClaveInicialViewModel
    {
        [Display(Name = "Nueva contraseña")]
        [Required(ErrorMessage = "Debe ingresar una nueva contraseña.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 carácteres.")]
        public string NuevaClave {  get; set; } = string.Empty;


        [Display(Name = "Confirmar contraseña")]
        [Required(ErrorMessage = "Debe confirmar la nueva contraseña.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NuevaClave), ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarClave {  get; set; } = string.Empty;
    }
}
