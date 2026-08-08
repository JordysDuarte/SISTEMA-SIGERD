using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGERD.Constants.Seguridad;
using SIGERD.Interfaces.IServices.Recepciones;
using SIGERD.Mappings;
using SIGERD.ViewModels.Recepciones;
using System.Security.Claims;

namespace SIGERD.Controllers.Recepciones
{
    [Authorize]
    public class RecepcionesController : BaseController
    {
        private readonly IRecepcionService _recepcionService;
        private readonly ILogger<RecepcionesController> _logger;

        public RecepcionesController(
            IRecepcionService recepcionService,
            ILogger<RecepcionesController> logger)
        {
            _recepcionService = recepcionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                int idDelegacionUsuario = ObtenerIdDelegacionActual();
                bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);

                var envios = await _recepcionService.ObtenerPendientesRecepcionAsync(
                    idDelegacionUsuario,
                    esSuperAdministrador
                );

                var model = envios
                    .Select(RecepcionMapper.ToListViewModel)
                    .ToList();

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al cargar los envíos pendientes de recepción.");

                MostrarError("No fue posible cargar los envíos pendientes de recepción.");

                return View(new List<RecepcionEnvioListViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Confirmar(int id)
        {
            if (id <= 0)
            {
                MostrarAdvertencia("El identificador del envío no es válido.");

                return RedirectToAction(nameof(Index));
            }

            try
            {
                int idDelegacionUsuario = ObtenerIdDelegacionActual();
                bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);

                var envio = await _recepcionService.ObtenerEnvioParaConfirmarAsync(
                    id,
                    idDelegacionUsuario,
                    esSuperAdministrador
                );

                if (envio is null)
                {
                    MostrarAdvertencia("El envío no existe, ya fue recibido o no tienes permiso para recibirlo.");

                    return RedirectToAction(nameof(Index));
                }

                var model = RecepcionMapper.ToConfirmViewModel(envio);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió un error al cargar la confirmación de recepción del envío {IdEnvio}.",
                    id
                );

                MostrarError("No fue posible cargar la confirmación de recepción.");

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmar(RecepcionConfirmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                int idDelegacionUsuario = ObtenerIdDelegacionActual();
                bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);

                var envio = await _recepcionService.ObtenerEnvioParaConfirmarAsync(
                    model.IdEnvio,
                    idDelegacionUsuario,
                    esSuperAdministrador
                );

                if (envio is not null)
                {
                    var modelRecargado = RecepcionMapper.ToConfirmViewModel(envio);
                    modelRecargado.Observaciones = model.Observaciones;

                    return View(modelRecargado);
                }

                return View(model);
            }

            int idUsuarioActual = ObtenerIdUsuarioActual();
            int idDelegacionActual = ObtenerIdDelegacionActual();
            bool esSuperAdmin = User.IsInRole(RolesSistema.SuperAdministrador);

            try
            {
                int idRecepcion = await _recepcionService.ConfirmarRecepcionAsync(
                    model.IdEnvio,
                    idUsuarioActual,
                    idDelegacionActual,
                    esSuperAdmin,
                    model.Observaciones
                );

                MostrarExito("La recepción fue registrada correctamente.");

                return RedirectToAction(nameof(Details), new { id = idRecepcion });
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
                    "Ocurrió un error al confirmar la recepción del envío {IdEnvio}.",
                    model.IdEnvio
                );

                MostrarError("No fue posible registrar la recepción.");

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                MostrarAdvertencia("El identificador de la recepción no es válido.");

                return RedirectToAction(nameof(Index));
            }

            try
            {
                int idDelegacionUsuario = ObtenerIdDelegacionActual();
                bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);

                var recepcion = await _recepcionService.ObtenerPorIdValidadoAsync(
                    id,
                    idDelegacionUsuario,
                    esSuperAdministrador
                );

                if (recepcion is null)
                {
                    MostrarAdvertencia("La recepción no existe o no tienes permiso para verla.");

                    return RedirectToAction(nameof(Index));
                }

                var model = RecepcionMapper.ToDetailsViewModel(recepcion);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió un error al cargar el detalle de la recepción {IdRecepcion}.",
                    id
                );

                MostrarError("No fue posible cargar el detalle de la recepción.");

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