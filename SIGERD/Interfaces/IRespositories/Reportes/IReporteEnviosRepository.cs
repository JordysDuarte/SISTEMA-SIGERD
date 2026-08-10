using SIGERD.DTOs.Reportes;
using SIGERD.Models.Envios;

namespace SIGERD.Interfaces.IRespositories.Reportes
{
    public interface IReporteEnviosRepository
    {
        Task<IEnumerable<Envio>> ObtenerEnviosAsync(ReporteEnviosFiltroDto filtro);
    }
}