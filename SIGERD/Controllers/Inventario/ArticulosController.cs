using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGERD.Constants.Seguridad;
using SIGERD.Interfaces.IServices.Common;
using SIGERD.Interfaces.IServices.Inventario;
using SIGERD.Mappings;
using SIGERD.ViewModels.Inventario.Articulos;

namespace SIGERD.Controllers.Inventario
{
    [Authorize(Roles = RolesSistema.SuperAdministrador)]
    public class ArticulosController : BaseController
    {
        private readonly IArticuloService _articuloService;
        private readonly ISelectListService _selectListService;
        private readonly ILogger<ArticulosController> _logger;

        public ArticulosController(
            IArticuloService articuloService,
            ISelectListService selectListService,
            ILogger<ArticulosController> logger)
        {
            _articuloService = articuloService;
            _selectListService = selectListService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articulos = await _articuloService.ObtenerTodosAsync();

            var model = articulos
                .Select(ArticuloMapper.ToListViewModel)
                .ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                MostrarAdvertencia("El identificador del artículo no es válido.");
                return RedirectToAction(nameof(Index));
            }

            var articulo = await _articuloService.ObtenerPorIdAsync(id);

            if (articulo is null)
            {
                MostrarAdvertencia("El artículo solicitado no existe.");
                return RedirectToAction(nameof(Index));
            }

            var model = ArticuloMapper.ToDetailsViewModel(articulo);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ArticuloCreateViewModel();

            await CargarCombosAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticuloCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombosAsync(model);
                return View(model);
            }

            try
            {
                var articulo = ArticuloMapper.ToEntity(model);

                await _articuloService.CrearAsync(articulo);

                MostrarExito("El artículo fue registrado correctamente.");

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarCombosAsync(model);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al crear un artículo.");

                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible registrar el artículo. Intenta nuevamente."
                );

                await CargarCombosAsync(model);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                MostrarAdvertencia("El identificador del artículo no es válido.");
                return RedirectToAction(nameof(Index));
            }

            var articulo = await _articuloService.ObtenerPorIdAsync(id);

            if (articulo is null)
            {
                MostrarAdvertencia("El artículo solicitado no existe.");
                return RedirectToAction(nameof(Index));
            }

            var model = ArticuloMapper.ToEditViewModel(articulo);

            await CargarCombosAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticuloEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombosAsync(model);
                return View(model);
            }

            try
            {
                var articulo = ArticuloMapper.ToEntity(model);

                await _articuloService.ActualizarAsync(articulo);

                MostrarExito("El artículo fue actualizado correctamente.");

                return RedirectToAction(nameof(Details), new { id = model.IdArticulo });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarCombosAsync(model);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió un error al actualizar el artículo {IdArticulo}.",
                    model.IdArticulo
                );

                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible actualizar el artículo. Intenta nuevamente."
                );

                await CargarCombosAsync(model);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, bool nuevoEstado)
        {
            try
            {
                await _articuloService.CambiarEstadoAsync(id, nuevoEstado);

                MostrarExito(
                    nuevoEstado
                        ? "El artículo fue activado correctamente."
                        : "El artículo fue desactivado correctamente."
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
                    "Ocurrió un error al cambiar el estado del artículo {IdArticulo}.",
                    id
                );

                MostrarError("No fue posible cambiar el estado del artículo.");
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarCombosAsync(ArticuloCreateViewModel model)
        {
            model.Categorias = await _selectListService.ObtenerCategoriasActivaAsync();
        }

        private async Task CargarCombosAsync(ArticuloEditViewModel model)
        {
            model.Categorias = await _selectListService.ObtenerCategoriasActivaAsync();
        }
    }
}