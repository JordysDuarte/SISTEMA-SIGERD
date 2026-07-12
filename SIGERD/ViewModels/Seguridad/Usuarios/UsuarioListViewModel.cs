namespace SIGERD.ViewModels.Seguridad.Usuarios
{
    public class UsuarioListViewModel
    {
        public int IdUsuario { get; set; }
        public string? NombreCompleto { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo {  get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Delegacion {  get; set; } = string.Empty;
        public bool Estado { get; set;  }
    }
}
