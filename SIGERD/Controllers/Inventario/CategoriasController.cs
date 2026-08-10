using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGERD.Constants.Seguridad;
using SIGERD.Interfaces.IServices.Inventario;
using SIGERD.Mappings;
using SIGERD.ViewModels.Inventario.Categorias;

namespace SIGERD.Controllers.Inventario
{
    [Authorize(Roles = RolesSistema.SuperAdministrador)]
    public class CategoriasController : BaseController
    {
        private readonly ICategoriaService _categoriaService;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(
            ICategoriaService categoriaService,
            ILogger<CategoriasController> logger)
        {
            _categoriaService = categoriaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.ObtenerTodosAsync();

            var model = categorias
                .Select(CategoriaMapper.ToListViewModel)
                .ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                MostrarAdvertencia("El identificador de la categoría no es válido.");
                return RedirectToAction(nameof(Index));
            }

            var categoria = await _categoriaService.ObtenerPorIdAsync(id);

            if (categoria is null)
            {
                MostrarAdvertencia("La categoría solicitada no existe.");
                return RedirectToAction(nameof(Index));
            }

            var model = CategoriaMapper.ToDetailsViewModel(categoria);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoriaCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var categoria = CategoriaMapper.ToEntity(model);

                await _categoriaService.CrearAsync(categoria);

                MostrarExito("La categoría fue registrada correctamente.");

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al crear una categoría.");

                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible registrar la categoría. Intenta nuevamente."
                );

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                MostrarAdvertencia("El identificador de la categoría no es válido.");
                return RedirectToAction(nameof(Index));
            }

            var categoria = await _categoriaService.ObtenerPorIdAsync(id);

            if (categoria is null)
            {
                MostrarAdvertencia("La categoría solicitada no existe.");
                return RedirectToAction(nameof(Index));
            }

            var model = CategoriaMapper.ToEditViewModel(categoria);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoriaEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var categoria = CategoriaMapper.ToEntity(model);

                await _categoriaService.ActualizarAsync(categoria);

                MostrarExito("La categoría fue actualizada correctamente.");

                return RedirectToAction(nameof(Details), new { id = model.IdCategoria });
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
                    "Ocurrió un error al actualizar la categoría {IdCategoria}.",
                    model.IdCategoria
                );

                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible actualizar la categoría. Intenta nuevamente."
                );

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, bool nuevoEstado)
        {
            try
            {
                await _categoriaService.CambiarEstadoAsync(id, nuevoEstado);

                MostrarExito(
                    nuevoEstado
                        ? "La categoría fue activada correctamente."
                        : "La categoría fue desactivada correctamente."
                );
            }
            catch (InvalidOperationException ex)
            {
                MostrarAdvertencia(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió un error al cambiar el estado de la categoría {IdCategoria}.",
                    id
                );

                MostrarError("No fue posible cambiar el estado de la categoría.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}