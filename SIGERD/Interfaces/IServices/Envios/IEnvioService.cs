using SIGERD.Models.Envios;

namespace SIGERD.Interfaces.IServices.Envios
{
    public interface IEnvioService
    {
        Task<IEnumerable<Envio>> ObtenerTodosAsync();

        Task<Envio?> ObtenerPorIdAsync(int idEnvio);

        Task CrearAsync(Envio envio);
    }
}
