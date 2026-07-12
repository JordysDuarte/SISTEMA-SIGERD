using SIGERD.Models.Seguridad;

namespace SIGERD.Interfaces.IServices.Seguridad
{
    public interface IRolService
    {
        Task<IEnumerable<Rol>> ObtenerTodosAsync();
        Task<Rol> ObtenerPorIdAsync(int id);
        Task CrearAsync(Rol rol);
        Task ActualizarAsync(Rol rol);
        Task EliminarAsync(int id);
    }
}
