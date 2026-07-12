using SIGERD.Interfaces.IData;
using SIGERD.Models.Seguridad;

namespace SIGERD.Interfaces.IRespositories.Seguridad
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario?> ObtenerPorCorreoAsync(string correo);

        Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);

        Task<bool> ExisteCorreoAsync(string correo);

        Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);
    }
}