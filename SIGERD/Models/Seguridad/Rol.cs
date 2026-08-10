namespace SIGERD.Models.Seguridad
{
    public class Rol
    {
        public int idRol { get; set; }
        public string nombreRol { get; set; } = string.Empty;
        public string? descripcion { get; set; }
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
