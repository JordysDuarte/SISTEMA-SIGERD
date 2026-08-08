using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGERD.Interfaces.IServices.Common;
using SIGERD.Interfaces.IServices.Envios;
using SIGERD.Mappings;
using System.Security.Claims;
using SIGERD.ViewModels.Envios.Envios;
using SIGERD.Constants.Envios;
using SIGERD.Constants.Seguridad;
using System.Security.Claims;

namespace SIGERD.Controllers.Envios
{
    [Authorize]
    public class EnviosController : BaseController
    {
        private const int FilasDetallePorDefecto = 5;

        private readonly IEnvioService _envioService;
        private readonly ISelectListService _selectListService;
        private readonly ILogger<EnviosController> _logger;

        public EnviosController(
            IEnvioService envioService,
            ISelectListService selectListService,
            ILogger<EnviosController> logger)
        {
            _envioService = envioService;
            _selectListService = selectListService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? tipoVista)
        {
            try
            {
                bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);
                int idDelegacionUsuario = ObtenerIdDelegacionActual();

                string tipoVistaNormalizada = TiposVistaEnvio.Normalizar(
                    tipoVista,
                    esSuperAdministrador
                );

                var envios = await _envioService.ObtenerPorVistaAsync(
                    tipoVistaNormalizada,
                    idDelegacionUsuario,
                    esSuperAdministrador
                );

                var model = new EnviosIndexViewModel
                {
                    TipoVistaActual = tipoVistaNormalizada,
                    EsSuperAdministrador = esSuperAdministrador,
                    Envios = envios
                        .Select(EnvioMapper.ToListViewModel)
                        .ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al cargar los envíos.");

                MostrarError("No fue posible cargar los envíos.");

                var model = new EnviosIndexViewModel
                {
                    TipoVistaActual = TiposVistaEnvio.Enviados,
                    EsSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador),
                    Envios = new List<EnvioListViewModel>()
                };

                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                MostrarAdvertencia("El identificador del envío no es válido.");

                return RedirectToAction(nameof(Index));
            }

            try
            {
                bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);
                int idDelegacionUsuario = ObtenerIdDelegacionActual();

                var envio = await _envioService.ObtenerPorIdValidadoAsync(
                    id,
                    idDelegacionUsuario,
                    esSuperAdministrador
                );

                if (envio is null)
                {
                    MostrarAdvertencia("El envío solicitado no existe o no tienes permiso para verlo.");

                    return RedirectToAction(nameof(Index));
                }

                var model = EnvioMapper.ToDetailsViewModel(envio);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió un error al cargar el detalle del envío con Id {IdEnvio}.",
                    id
                );

                MostrarError("No fue posible cargar el detalle del envío.");

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new EnvioCreateViewModel();

            InicializarDetalles(model, FilasDetallePorDefecto);
            ConfigurarOrigenSegunUsuario(model);

            await CargarCombosAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnvioCreateViewModel model)
        {
            bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);
            int idDelegacionUsuario = ObtenerIdDelegacionActual();

            ConfigurarOrigenSegunUsuario(model);

            if (!esSuperAdministrador)
            {
                ModelState.Remove(nameof(model.IdDelegacionOrigen));
            }

            LimpiarDetallesVacios(model);
            ValidarFormulario(model);

            if (!ModelState.IsValid)
            {
                AsegurarCantidadMinimaDeFilas(model, FilasDetallePorDefecto);
                await CargarCombosAsync(model);

                return View(model);
            }

            try
            {
                int idUsuarioActual = ObtenerIdUsuarioActual();

                var envio = EnvioMapper.ToEntity(model, idUsuarioActual);

                int idEnvio = await _envioService.CrearAsync(
                    envio,
                    idDelegacionUsuario,
                    esSuperAdministrador
                );

                MostrarExito("El envío fue registrado correctamente.");

                return RedirectToAction(nameof(Details), new { id = idEnvio });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                AsegurarCantidadMinimaDeFilas(model, FilasDetallePorDefecto);
                await CargarCombosAsync(model);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al registrar un envío.");

                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible registrar el envío. Intenta nuevamente."
                );

                AsegurarCantidadMinimaDeFilas(model, FilasDetallePorDefecto);
                await CargarCombosAsync(model);

                return View(model);
            }
        }

        #region Métodos privados

        private int ObtenerIdUsuarioActual()
        {
            string? idUsuarioClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(idUsuarioClaim, out int idUsuario))
            {
                return idUsuario;
            }

            return 0;
        }

        private void InicializarDetalles(EnvioCreateViewModel model, int cantidad)
        {
            model.Detalles = new List<DetalleEnvioCreateViewModel>();

            for (int i = 0; i < cantidad; i++)
            {
                model.Detalles.Add(new DetalleEnvioCreateViewModel());
            }
        }

        private void AsegurarCantidadMinimaDeFilas(EnvioCreateViewModel model, int minimo)
        {
            model.Detalles ??= new List<DetalleEnvioCreateViewModel>();

            while (model.Detalles.Count < minimo)
            {
                model.Detalles.Add(new DetalleEnvioCreateViewModel());
            }
        }

        private void LimpiarDetallesVacios(EnvioCreateViewModel model)
        {
            model.Detalles ??= new List<DetalleEnvioCreateViewModel>();

            model.Detalles = model.Detalles
                .Where(d => 
                    d.IdArticulo.HasValue || 
                    d.Cantidad.HasValue ||
                    !string.IsNullOrWhiteSpace(d.ObservacionesDetalle))
                .ToList();
        }

        private void ValidarFormulario(EnvioCreateViewModel model)
        {
            if (model.IdDelegacionOrigen > 0 &&
                model.IdDelegacionDestino > 0 &&
                model.IdDelegacionOrigen == model.IdDelegacionDestino)
            {
                ModelState.AddModelError(nameof(model.IdDelegacionDestino), "La delegación origen y destino no pueden ser la misma.");
            }

            if (model.Detalles == null || !model.Detalles.Any())
            {
                ModelState.AddModelError(string.Empty, "Debe agregar al menos un artículo al envío.");
                return;
            }

            for (int i = 0; i < model.Detalles.Count; i++)
            {
                var detalle = model.Detalles[i];

                bool tieneArticulo = detalle.IdArticulo.HasValue && detalle.IdArticulo.Value > 0;
                bool tieneCantidad = detalle.Cantidad.HasValue && detalle.Cantidad.Value > 0;
                bool tieneDescripcion = !string.IsNullOrWhiteSpace(detalle.ObservacionesDetalle);

                if (tieneArticulo && !tieneCantidad)
                {
                    ModelState.AddModelError($"Detalles[{i}].Cantidad", "Debe ingresar una cantidad válida.");
                }

                if (!tieneArticulo && tieneCantidad)
                {
                    ModelState.AddModelError($"Detalles[{i}].IdArticulo", "Debe seleccionar un artículo.");
                }

                if (!tieneArticulo && tieneDescripcion)
                {
                    ModelState.AddModelError($"Detalles[{i}].IdArticulo", "Debe seleccionar un artículo.");
                }

               if (tieneArticulo && detalle.Cantidad.HasValue && detalle.Cantidad.Value <= 0)
                {
                    ModelState.AddModelError($"Detalles[{i}].Cantidad", "La cantidad debe ser mayor a cero.");
                }

               if (detalle.IdArticulo.HasValue && detalle.IdArticulo.Value <= 0)
                {
                    ModelState.AddModelError($"Detalles[{i}].Cantidad", "La cantidad debe ser mayor que cero.");
                }
            }

            var articulosRepetidos = model.Detalles
                .Where(d => d.IdArticulo.HasValue && d.IdArticulo.Value > 0)
                .GroupBy(d => d.IdArticulo!.Value)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (articulosRepetidos.Any())
            {
                ModelState.AddModelError(string.Empty, "No se permite repetir el mismo artículo en el mismo envío.");
            }
        }

        private async Task CargarCombosAsync(EnvioCreateViewModel model)
        {
            var delegaciones = (await _selectListService.ObtenerDelegacionesAsync()).ToList();

            if (!model.EsSuperAdministrador && model.IdDelegacionOrigen > 0)
            {
                model.Delegaciones = delegaciones
                    .Where(d => d.Value != model.IdDelegacionOrigen.ToString())
                    .ToList();
            }
            else
            {
                model.Delegaciones = delegaciones;
            }

            var articulos = await _selectListService.ObtenerArticulosAsync();

            model.Articulos = articulos;

            model.Detalles ??= new List<DetalleEnvioCreateViewModel>();

            foreach (var detalle in model.Detalles)
            {
                detalle.Articulos = articulos;
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

        private void ConfigurarOrigenSegunUsuario(EnvioCreateViewModel model)
        {
            bool esSuperAdministrador = User.IsInRole(RolesSistema.SuperAdministrador);

            model.EsSuperAdministrador = esSuperAdministrador;
            model.DelegacionOrigenUsuario = User.FindFirstValue("Delegacion") ?? "Delegación asignada";

            if (!esSuperAdministrador)
            {
                model.IdDelegacionOrigen = ObtenerIdDelegacionActual();
            }
        }

        #endregion
    }
}