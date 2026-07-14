using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SIGERD.DTOs.Auth;
using SIGERD.Interfaces.IServices.Seguridad;
using SIGERD.ViewModels.Auth;
using System.Security.Claims;

namespace SIGERD.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }






        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                ResultadoAutenticacionDto resultado =
                    await _authService.AutenticarAsync(
                        model.NombreUsuario,
                        model.Clave
                    );

                if (!resultado.Exitoso || resultado.Usuario is null)
                {
                    ModelState.AddModelError(string.Empty, resultado.Mensaje);

                    return View(model);
                }

                await CrearSesionAsync(resultado.Usuario, model.Recordarme);

                if (resultado.Usuario.DebeCambiarClave)
                {
                    return RedirectToAction(nameof(CambiarClaveInicial));
                }

                return RedireccionarDespuesDelLogin(model.ReturnUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió un error al iniciar sesión con el usuario {NombreUsuario}.",
                    model.NombreUsuario
                );

                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible iniciar sesión. Intenta nuevamente."
                );

                return View(model);
            }
        }



        [HttpGet]
        [Authorize]
        public IActionResult CambiarClaveInicial()
        {
            bool debeCambiarClave = User.FindFirst("DebeCambiarClave")?.Value == "true";

            if (!debeCambiarClave)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View(new CambiarClaveInicialViewModel());
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarClaveInicial(CambiarClaveInicialViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string? idUsuarioClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!int.TryParse(idUsuarioClaim, out int idUsuario))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return RedirectToAction(nameof(Login));
            }

            try
            {
                await _authService.CambiarClaveInicialAsync(idUsuario, model.NuevaClave);

                MostrarExito("La contraseña fue actualizada correctamente. Inicie sesión nuevamente");

                return RedirectToAction(nameof(Login));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al cambiar la contraseña inicial del usuario {IdUsuario}", idUsuario);

                ModelState.AddModelError(string.Empty, "No fue posible actualizar la constraseña. Intenta nuevamente");

                return View(model);
            }
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


        private async Task CrearSesionAsync(UsuarioSesionDto usuario, bool recordarSesion)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Role, usuario.Rol),

                new Claim("NombreUsuario", usuario.NombreCompleto),
                new Claim("IdDelegacion", usuario.IdDelegacion.ToString()),
                new Claim("Delegacion", usuario.Delegacion),
                new Claim("DebeCambiarClave", usuario.DebeCambiarClave.ToString().ToLower()),
                new Claim("VersionSeguridad", usuario.VersionSeguridad.ToString())
            };

            var identity = new ClaimsIdentity(
               claims,
               CookieAuthenticationDefaults.AuthenticationScheme
           );

            var principal = new ClaimsPrincipal(identity);

            var propiedades = new AuthenticationProperties
            {
                IsPersistent = recordarSesion
            };

            if (recordarSesion)
            {
                propiedades.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7);
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                propiedades
            );
        }

        private IActionResult RedireccionarDespuesDelLogin(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Dashboard");
        }

    }
}
