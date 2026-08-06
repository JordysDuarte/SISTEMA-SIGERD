using Microsoft.AspNetCore.Mvc;
using SIGERD.Constants.Seguridad;
using SIGERD.Interfaces.IServices.Despachos;
using SIGERD.Mappings;
using SIGERD.ViewModels.Despachos;
using System.Security.Claims;

namespace SIGERD.Controllers.Despachos
{
    public class DespachosController : BaseController
    {
        private readonly IDespachoService _despachoService;
        private readonly ILogger<DespachosController> _logger;

        public DespachosController(
            IDespachoService despachoService,
            ILogger<DespachosController> logger)
        {
            _despachoService = despachoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                int idDelegacionUsuario = ObtenerIdDelegacionActual();
                bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);

                var enviosPendientes = await _despachoService.ObtenerPendientesAsync(
                    idDelegacionUsuario,
                    esSuperAdministrador
                );

                var model = enviosPendientes
                    .Select(DespachoMapper.ToListViewModel)
                    .ToList();

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al cargar los envíos pendientes de despacho.");

                MostrarError("No fue posible cargar los envíos pendientes de despacho.");

                return View(new List<DespachoListViewModel>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Despachar(int id)
        {
            if (id <= 0)
            {
                MostrarAdvertencia("El identificador del envío no es válido.");

                return RedirectToAction(nameof(Index));
            }

            int idUsuarioActual = ObtenerIdUsuarioActual();
            int idDelegacionUsuario = ObtenerIdDelegacionActual();
            bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);

            try
            {
                await _despachoService.DespacharAsync(
                    id,
                    idUsuarioActual,
                    idDelegacionUsuario,
                    esSuperAdministrador
                );

                MostrarExito("El envío fue despachado correctamente.");

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                MostrarAdvertencia(ex.Message);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió un error al despachar el envío con Id {IdEnvio}.",
                    id
                );

                MostrarError("No fue posible despachar el envío.");

                return RedirectToAction(nameof(Index));
            }
        }

        private int ObtenerIdUsuarioActual()
        {
            string? idUsuarioClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(idUsuarioClaim, out int idUsuario))
            {
                return idUsuario;
            }

            return 0;
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
