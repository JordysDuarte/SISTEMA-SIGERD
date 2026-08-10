namespace SIGERD.ViewModels.Seguridad.Roles
{
    public class RolDetailsViewModel
    {
        public int IdRol { get; set; }

        public string NombreRol { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool EsRolBase { get; set; }
    }
}