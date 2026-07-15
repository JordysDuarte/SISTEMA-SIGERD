using Microsoft.AspNetCore.Mvc;
using SIGERD.Interfaces.IServices.Dashboard;
using SIGERD.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;

namespace SIGERD.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {

        #region Campos

        private readonly IDashboardService _dashboardService;

        #endregion

        #region Constructor

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        #endregion

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await _dashboardService.ObtenerResumenAsync();

                return View(model);
            }
            catch
            {
                MostrarError("No fue posible cargar la informacion del Dashboard.");

                DashboardViewModel model = new DashboardViewModel();

                return View();
            }
        }

        #endregion
    }
}
