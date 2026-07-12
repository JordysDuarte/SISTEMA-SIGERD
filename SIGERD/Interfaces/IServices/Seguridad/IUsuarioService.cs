using SIGERD.Models.Seguridad;

namespace SIGERD.Interfaces.IServices.Seguridad
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();

        Task<Usuario?> ObtenerPorIdAsync(int id);

        Task<Usuario?> ObtenerPorCorreoAsync(string correo);

        Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);

        Task<bool> ExisteCorreoAsync(string correo);

        Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);

        Task CrearAsync(Usuario usuario, string claveInicial);

        Task ActualizarAsync(Usuario usuario);

        Task EliminarAsync(int id);
    }
}