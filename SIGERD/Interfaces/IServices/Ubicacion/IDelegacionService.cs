using SIGERD.Models.Ubicacion;

namespace SIGERD.Interfaces.IServices.Ubicacion
{
    public interface IDelegacionService
    {
        Task<IEnumerable<Delegacion>> ObtenerTodasAsync();

        Task<bool> ExisteAsync(int idDelegacion);
    }
}
