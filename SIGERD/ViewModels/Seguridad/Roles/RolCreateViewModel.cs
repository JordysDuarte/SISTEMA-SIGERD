using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Seguridad.Roles
{
    public class RolCreateViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el nombre del rol.")]
        [StringLength(50, ErrorMessage = "El nombre del rol no debe superar los 50 caracteres.")]
        [Display(Name = "Nombre del rol")]
        public string NombreRol { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "La descripción no debe superar los 150 carácteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
}