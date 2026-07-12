using SIGERD.Interfaces.IData;
using SIGERD.Models.Seguridad;
using SIGERD.ViewModels;

namespace SIGERD.Interfaces.IRespositories.Seguridad
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario?> ObtenerPorCorreoAsync(string correo);
        Task<bool> ExisteCorreoAsync(string correo);
        Task<Usuario?> ValidarCredencialesAsync(string correo, string clave); 
    }
}
