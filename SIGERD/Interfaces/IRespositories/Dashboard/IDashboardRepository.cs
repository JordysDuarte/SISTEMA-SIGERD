using SIGERD.Models.Envios;

namespace SIGERD.Interfaces.IRespositories.Dashboard
{
    public interface IDashboardRepository
    {
        Task<int> ContarUsuariosAsync();

        Task<int> ContarArticulosAsync();

        Task<int> ContarEnviosAsync();

        Task<int> ContarRecepcionesAsync();

        Task<int> ContarEnviosEnviadosPorDelegacionAsync(int idDelegacion);

        Task<int> ContarEnviosDestinadosADelegacionAsync(int idDelegacion);

        Task<int> ContarPendientesDespachoAsync(int? idDelegacionOrigen = null);

        Task<int> ContarPendientesRecepcionAsync(int? idDelegacionDestino = null);

        Task<int> ContarEnviosEnTransitoAsync(int? idDelegacionRelacionada = null);

        Task<int> ContarEnviosRecibidosAsync(int? idDelegacionRelacionada = null);

        Task<IEnumerable<Envio>> ObtenerUltimosEnviosAsync(int cantidad);

        Task<IEnumerable<Envio>> ObtenerUltimosEnviosRelacionadosAsync(int idDelegacion, int cantidad);
    }
}