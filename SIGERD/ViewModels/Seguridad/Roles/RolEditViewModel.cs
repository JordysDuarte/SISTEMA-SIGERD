using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Seguridad.Roles
{
    public class RolEditViewModel
    {
        public int IdRol { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del rol.")]
        [StringLength(50, ErrorMessage = "El nombre del rol no debe superar los 50 caracteres.")]
        [Display(Name = "Nombre del rol")]
        public string NombreRol { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no debe superar los 500 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        public bool EsRolBase { get; set; }
    }
}