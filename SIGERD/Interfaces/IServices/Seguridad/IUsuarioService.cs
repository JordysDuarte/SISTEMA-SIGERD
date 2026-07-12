using SIGERD.Models.Seguridad;

namespace SIGERD.Interfaces.IServices.Seguridad
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario?> ObtenerPorCorreoAsync(string correo);
        Task<bool> ExisteCorreoAsync(string correo);
        Task<Usuario?> ValidarCredencialesAsync(string correo, string clave);
        Task CrearAsync(Usuario usuario);
        Task ActualizarAsync(Usuario usuario);
        Task EliminarAsync(int id);
    }
}
