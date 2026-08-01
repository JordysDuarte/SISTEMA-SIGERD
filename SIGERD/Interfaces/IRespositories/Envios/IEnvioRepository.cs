using SIGERD.Models.Envios;

namespace SIGERD.Interfaces.IRespositories.Envios
{
    public interface IEnvioRepository
    {
        Task<IEnumerable<Envio>> ObtenerTodosAsync();

        Task<Envio?> ObtenerPorIdAsync(int idEnvio);

        Task AgregarAsync(Envio envio);

        void Actualizar(Envio envio);

        Task GuardarAsync();

        Task<int> ObtenerConsecutivoDiarioAsync(DateTime fecha);
    }
}
