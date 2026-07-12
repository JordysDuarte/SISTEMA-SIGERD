using SIGERD.Interfaces.IData;
using SIGERD.Models.Ubicacion;

namespace SIGERD.Interfaces.IRespositories.Ubicacion
{
    public interface IDelegacionRepository : IGenericRepository<Delegacion>
    {
        Task<bool> ExisteAsync(int idDelegacion);
    }
}