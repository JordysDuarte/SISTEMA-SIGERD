using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGERD.Constants.Seguridad;
using SIGERD.Interfaces.IServices.Dashboard;
using SIGERD.ViewModels.Dashboard;
using System.Security.Claims;

namespace SIGERD.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IDashboardService dashboardService,
            ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                int idDelegacionUsuario = ObtenerIdDelegacionActual();
                string nombreUsuario = User.Identity?.Name ?? "Usuario";
                string nombreDelegacion = User.FindFirstValue("Delegacion") ?? "Sin delegación";
                bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);

                var model = await _dashboardService.ObtenerDashboardAsync(
                    idDelegacionUsuario,
                    nombreUsuario,
                    nombreDelegacion,
                    esSuperAdministrador
                );

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al cargar el Dashboard.");

                MostrarError("No fue posible cargar la información del Dashboard.");

                return View(new DashboardViewModel());
            }
        }

        private int ObtenerIdDelegacionActual()
        {
            string? idDelegacionClaim = User.FindFirstValue("IdDelegacion");

            if (int.TryParse(idDelegacionClaim, out int idDelegacion))
            {
                return idDelegacion;
            }

            return 0;
        }
    }
}