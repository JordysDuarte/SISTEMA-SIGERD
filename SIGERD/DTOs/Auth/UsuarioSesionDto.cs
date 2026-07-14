namespace SIGERD.DTOs.Auth
{
    public class UsuarioSesionDto
    {
        public int IdUsuario {  get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public int IdDelegacion { get; set; }
        public string Delegacion {  get; set; } = string.Empty;
        public bool DebeCambiarClave { get; set; }
        public Guid VersionSeguridad { get; set; }
    }
}
