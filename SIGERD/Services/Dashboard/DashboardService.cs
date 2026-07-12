using SIGERD.Interfaces.IServices.Dashboard;
using SIGERD.ViewModels.Dashboard;
using SIGERD.Interfaces.IRespositories.Dashboard;

namespace SIGERD.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {

        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardViewModel> ObtenerResumenAsync()
        {
            return await _dashboardRepository.ObtenerResumenAsync();
        }
    }
}
