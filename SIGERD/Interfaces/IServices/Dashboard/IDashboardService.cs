using SIGERD.ViewModels.Dashboard;

namespace SIGERD.Interfaces.IServices.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> ObtenerDashboardAsync(
            int idDelegacionUsuario,
            string nombreUsuario,
            string nombreDelegacion,
            bool esSuperAdministrador
        );
    }
}