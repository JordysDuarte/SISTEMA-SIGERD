using SIGERD.Interfaces.IRespositories.Ubicacion;
using SIGERD.Interfaces.IServices.Ubicacion;
using SIGERD.Models.Ubicacion;

namespace SIGERD.Services.Ubicacion
{
    public class DelegacionService : IDelegacionService
    {
        private readonly IDelegacionRepository _delegacionRepository;

        public DelegacionService(IDelegacionRepository delegacionRepository)
        {
            _delegacionRepository = delegacionRepository;
        }

        public async Task<IEnumerable<Delegacion>> ObtenerTodasAsync()
        {
            return await _delegacionRepository.ObtenerTodosAsync();
        }

        public async Task<bool> ExisteAsync(int idDelegacion)
        {
            return await _delegacionRepository.ExisteAsync(idDelegacion);
        }
    }
}