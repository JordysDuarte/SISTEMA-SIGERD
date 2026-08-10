using SIGERD.Models.Seguridad;

namespace SIGERD.Interfaces.IRespositories.Seguridad
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> ObtenerTodosAsync();

        Task<Rol?> ObtenerPorIdAsync(int idRol);

        Task<Rol?> ObtenerPorNombreAsync(string nombreRol);

        Task<bool> ExisteNombreAsync(string nombreRol, int? idRolExcluir = null);

        Task AgregarAsync(Rol rol);

        void Actualizar(Rol rol);

        Task GuardarAsync();
    }
}