using SIGERD.ViewModels.Dashboard;
namespace SIGERD.Interfaces.IRespositories.Dashboard
{
    public interface IDashboardRepository
    {
        Task<DashboardViewModel> ObtenerResumenAsync();
    }
}
