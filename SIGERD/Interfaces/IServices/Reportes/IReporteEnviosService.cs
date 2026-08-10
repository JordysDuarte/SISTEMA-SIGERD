using SIGERD.DTOs.Reportes;
using SIGERD.ViewModels.Reportes.Envios;

namespace SIGERD.Interfaces.IServices.Reportes
{
    public interface IReporteEnviosService
    {
        Task<ReporteEnviosIndexViewModel> ObtenerReporteAsync(ReporteEnviosFiltroDto filtro);
    }
}