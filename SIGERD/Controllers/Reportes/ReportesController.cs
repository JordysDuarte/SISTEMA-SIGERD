using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGERD.Constants.Seguridad;
using SIGERD.DTOs.Reportes;
using SIGERD.Interfaces.IServices.Common;
using SIGERD.Interfaces.IServices.Reportes;
using SIGERD.ViewModels.Reportes.Envios;
using System.Security.Claims;

namespace SIGERD.Controllers.Reportes
{
    [Authorize]
    public class ReportesController : BaseController
    {
        private readonly IReporteEnviosService _reporteEnviosService;
        private readonly ISelectListService _selectListService;
        private readonly ILogger<ReportesController> _logger;

        public ReportesController(
            IReporteEnviosService reporteEnviosService,
            ISelectListService selectListService,
            ILogger<ReportesController> logger)
        {
            _reporteEnviosService = reporteEnviosService;
            _selectListService = selectListService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Envios(ReporteEnviosFiltroViewModel filtro)
        {
            try
            {
                bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);
                int idDelegacionUsuario = ObtenerIdDelegacionActual();

                var filtroDto = new ReporteEnviosFiltroDto
                {
                    FechaInicio = filtro.FechaInicio,
                    FechaFin = filtro.FechaFin,
                    IdDelegacionOrigen = filtro.IdDelegacionOrigen,
                    IdDelegacionDestino = filtro.IdDelegacionDestino,
                    IdEstadoEnvio = filtro.IdEstadoEnvio,
                    IdDelegacionUsuario = idDelegacionUsuario,
                    EsSuperAdministrador = esSuperAdministrador
                };

                var model = await _reporteEnviosService.ObtenerReporteAsync(filtroDto);

                model.Filtro = filtro;

                await CargarCombosAsync(model.Filtro, esSuperAdministrador, idDelegacionUsuario);

                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                MostrarAdvertencia(ex.Message);

                var model = new ReporteEnviosIndexViewModel
                {
                    Filtro = filtro,
                    Resultados = new List<ReporteEnviosResultadoViewModel>()
                };

                await CargarCombosAsync(
                    model.Filtro,
                    User.IsInRole(RolesSistema.SuperAdministrador),
                    ObtenerIdDelegacionActual()
                );

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al cargar el reporte de envíos.");

                MostrarError("No fue posible cargar el reporte de envíos.");

                var model = new ReporteEnviosIndexViewModel
                {
                    Filtro = filtro,
                    Resultados = new List<ReporteEnviosResultadoViewModel>()
                };

                await CargarCombosAsync(
                    model.Filtro,
                    User.IsInRole(RolesSistema.SuperAdministrador),
                    ObtenerIdDelegacionActual()
                );

                return View(model);
            }
        }

        private async Task CargarCombosAsync(
            ReporteEnviosFiltroViewModel filtro,
            bool esSuperAdministrador,
            int idDelegacionUsuario)
        {
            var delegaciones = (await _selectListService.ObtenerDelegacionesAsync()).ToList();

            if (!esSuperAdministrador && idDelegacionUsuario > 0)
            {
                delegaciones = delegaciones
                    .Where(d => d.Value == idDelegacionUsuario.ToString())
                    .ToList();
            }

            filtro.Delegaciones = delegaciones;
            filtro.EstadosEnvio = await _selectListService.ObtenerEstadosEnvioAsync();
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