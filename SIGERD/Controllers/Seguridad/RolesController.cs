using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGERD.Constants.Seguridad;
using SIGERD.Interfaces.IServices.Seguridad;
using SIGERD.Mappings;
using SIGERD.ViewModels.Seguridad.Roles;

namespace SIGERD.Controllers.Seguridad
{
    [Authorize(Roles = RolesSistema.SuperAdministrador)]
    public class RolesController : BaseController
    {
        private readonly IRolService _rolService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(
            IRolService rolService,
            ILogger<RolesController> logger)
        {
            _rolService = rolService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _rolService.ObtenerTodosAsync();

            var model = roles
                .Select(RolMapper.ToListViewModel)
                .ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                MostrarAdvertencia("El identificador del rol no es válido.");
                return RedirectToAction(nameof(Index));
            }

            var rol = await _rolService.ObtenerPorIdAsync(id);

            if (rol is null)
            {
                MostrarAdvertencia("El rol solicitado no existe.");
                return RedirectToAction(nameof(Index));
            }

            var model = RolMapper.ToDetailsViewModel(rol);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new RolCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RolCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var rol = RolMapper.ToEntity(model);

                await _rolService.CrearAsync(rol);

                MostrarExito("El rol fue registrado correctamente.");

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al crear un rol.");

                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible registrar el rol. Intenta nuevamente."
                );

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                MostrarAdvertencia("El identificador del rol no es válido.");
                return RedirectToAction(nameof(Index));
            }

            var rol = await _rolService.ObtenerPorIdAsync(id);

            if (rol is null)
            {
                MostrarAdvertencia("El rol solicitado no existe.");
                return RedirectToAction(nameof(Index));
            }

            var model = RolMapper.ToEditViewModel(rol);

            if (model.EsRolBase)
            {
                MostrarAdvertencia("Los roles base del sistema no se pueden editar.");
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RolEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var rol = RolMapper.ToEntity(model);

                await _rolService.ActualizarAsync(rol);

                MostrarExito("El rol fue actualizado correctamente.");

                return RedirectToAction(nameof(Details), new { id = model.IdRol });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió un error al actualizar el rol {IdRol}.",
                    model.IdRol
                );

                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible actualizar el rol. Intenta nuevamente."
                );

                return View(model);
            }
        }
    }
}