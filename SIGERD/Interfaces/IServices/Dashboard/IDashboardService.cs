using SIGERD.ViewModels.Dashboard;

namespace SIGERD.Interfaces.IServices.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> ObtenerResumenAsync();
    }
}
