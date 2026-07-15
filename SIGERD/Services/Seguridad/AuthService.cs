using Microsoft.AspNetCore.Identity;
using SIGERD.DTOs.Auth;
using SIGERD.Interfaces.IServices.Seguridad;
using SIGERD.Interfaces.IRespositories.Seguridad;
using SIGERD.Models.Seguridad;

namespace SIGERD.Services.Seguridad
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public AuthService(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResultadoAutenticacionDto> AutenticarAsync(string nombreUsuario, string clave)
        {
            nombreUsuario = nombreUsuario.Trim().ToLower();

            var usuario = await _usuarioRepository
                .ObtenerPorNombreUsuarioAsync(nombreUsuario);

            if(usuario is null)
            {
                return ResultadoAutenticacionDto.Fallido("Nombre de usuario o contraseña incorrectos.");
            }

            if (!usuario.estado)
            {
                return ResultadoAutenticacionDto.Fallido("La cuenta se encuentra inactiva. Contacte al administrador.");
            }

            PasswordVerificationResult resultadoVerificacion;

            try
            {
                resultadoVerificacion = _passwordHasher.VerifyHashedPassword(usuario, usuario.claveHash, clave);
            }
            catch
            {
                return ResultadoAutenticacionDto.Fallido("Nombre de usuario o contraseña incorrectos.");
            }

            if(resultadoVerificacion == PasswordVerificationResult.Failed)
            {
                return ResultadoAutenticacionDto.Fallido("Nombre de usuario o contraseña incorrectos.");
            }

            if (resultadoVerificacion == PasswordVerificationResult.SuccessRehashNeeded)
            {
                usuario.claveHash = _passwordHasher.HashPassword(usuario, clave);

                _usuarioRepository.Actualizar(usuario);

                await _usuarioRepository.GuardarAsync();
            }



            var usuarioSesion = new UsuarioSesionDto
            {
                IdUsuario = usuario.idUsuario,
                NombreCompleto = usuario.nombreCompleto,
                NombreUsuario = usuario.nombreUsuario,
                Rol = usuario.Rol?.nombreRol ?? string.Empty,
                IdDelegacion = usuario.idDelegacionUsuario,
                Delegacion = usuario.Delegacion?.nombreDelegacion ?? string.Empty,
                DebeCambiarClave = usuario.debeCambiarClave,
                VersionSeguridad = usuario.versionSeguridad
            };

            return ResultadoAutenticacionDto.Correcto(usuarioSesion);
        }


         public async Task CambiarClaveInicialAsync(int idUsuario, string nuevaClave)
        {
            if(idUsuario <= 0)
            {
                throw new InvalidOperationException("El usuario autenticado no es válido");
            }

            if (string.IsNullOrWhiteSpace(nuevaClave))
            {
                throw new InvalidOperationException("La nueva contraseña es obligatoria.");
            }

            if(nuevaClave.Length < 8)
            {
                throw new InvalidOperationException("La contraseña deve tener al menos 8 carácteres.");
            }

            var usuario = await _usuarioRepository.ObtenerPorIdAsync(idUsuario);
            if (usuario is null)
            {
                throw new InvalidOperationException("El usuario no existe o ya no está disponible.");
            }

            usuario.claveHash = _passwordHasher.HashPassword(usuario, nuevaClave);
            usuario.debeCambiarClave = false;
            usuario.fechaUltimoCambioClave = DateTime.UtcNow;
            usuario.versionSeguridad = Guid.NewGuid();

            _usuarioRepository.Actualizar(usuario);

            await _usuarioRepository.GuardarAsync();
        }
    }
}
