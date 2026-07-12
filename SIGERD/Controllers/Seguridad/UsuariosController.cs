using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SIGERD.Controllers;
using SIGERD.Interfaces.IServices.Seguridad;
using SIGERD.Interfaces.IServices.Common;
using SIGERD.Mappings;
using SIGERD.ViewModels.Seguridad.Usuarios;

namespace SIGERD.Controllers.Seguridad
{
    public class UsuariosController : BaseController
    {
        #region Campos

        private readonly IUsuarioService _usuarioService;
        private readonly ISelectListService _selectListService;
        private readonly ILogger<UsuariosController> _logger;

        #endregion

        #region Constructor

        public UsuariosController(IUsuarioService usuarioService, 
                                  ISelectListService selectListService,
                                  ILogger<UsuariosController> logger)
        {
            _usuarioService = usuarioService;
            _selectListService = selectListService;
            _logger = logger;   
        }

        #endregion

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarios = await _usuarioService.ObtenerTodosAsync();

                var model = usuarios
                    .Select(UsuarioMapper.ToListViewModel)
                    .ToList();

                return View(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió un error al obtener el listado de usuarios.");

                MostrarError("No fue posible cargar el listado de usuarios");

                return View(new List<UsuarioListViewModel>());
            }
        }

        #endregion


        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                MostrarAdvertencia("El identificador del usuario no es válido.");

                return RedirectToAction(nameof(Index));
            }

            try
            {
                var usuario = await _usuarioService.ObtenerPorIdAsync(id);
                if(usuario is null)
                {
                    MostrarAdvertencia("El usuario solicitado no existe o ya no está disponible");
                    return RedirectToAction(nameof(Index));
                }

                var model = UsuarioMapper.ToDetailsViewModel(usuario);

                return View(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,
                                 "Ocurrió un error al consultar el detalle del usuario con Id {IdUsuario}", id);

                return RedirectToAction(nameof(Index));
            }
        }

        #endregion


        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new UsuarioCreateViewModel();

            await CargarCombosUsuarioAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombosUsuarioAsync(model);

                return View(model);
            }

            try
            {
                var usuario = UsuarioMapper.ToEntity(model);

                await _usuarioService.CrearAsync(usuario, model.Clave);

                MostrarExito("El usuario fue registrado correctamente.");

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                await CargarCombosUsuarioAsync(model);

                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió un error al crear el usuario {NombreUsuario}.",
                    model.NombreUsuario
                );

                await CargarCombosUsuarioAsync(model);

                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible registrar el usuario. Intenta nuevamente."
                );

                return View(model);
            }
        }

        private async Task CargarCombosUsuarioAsync(UsuarioCreateViewModel model)
        {
            model.Roles = await _selectListService.ObtenerRolesAsync();

            model.Delegaciones = await _selectListService.ObtenerDelegacionesAsync();
        }

        #endregion
    }
}
