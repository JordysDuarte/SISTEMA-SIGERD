using SIGERD.Models.Envios;
using SIGERD.Models.Recepciones;

namespace SIGERD.Interfaces.IRespositories.Recepciones
{
    public interface IRecepcionRepository
    {
        Task<IEnumerable<Envio>> ObtenerEnviosEnTransitoAsync(int? idDelegacionDestino = null);

        Task<Envio?> ObtenerEnvioPorIdAsync(int idEnvio);

        Task<Recepcion?> ObtenerPorIdAsync(int idRecepcion);

        Task<Recepcion?> ObtenerPorEnvioAsync(int idEnvio);

        Task<int?> ObtenerIdEstadoPorNombreAsync(string nombreEstado);

        Task AgregarAsync(Recepcion recepcion);

        void ActualizarEnvio(Envio envio);

        Task GuardarAsync();
    }
}