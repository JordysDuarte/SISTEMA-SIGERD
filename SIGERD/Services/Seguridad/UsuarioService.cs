using Microsoft.AspNetCore.Identity;
using SIGERD.Interfaces.IRespositories.Seguridad;
using SIGERD.Interfaces.IServices.Seguridad;
using SIGERD.Models.Seguridad;

namespace SIGERD.Services.Seguridad
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _usuarioRepository.ObtenerTodosAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _usuarioRepository.ObtenerPorIdAsync(id);
        }

        public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
        {
            correo = correo.Trim().ToLower();

            return await _usuarioRepository.ObtenerPorCorreoAsync(correo);
        }

        public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
        {
            nombreUsuario = nombreUsuario.Trim().ToLower();

            return await _usuarioRepository.ObtenerPorNombreUsuarioAsync(nombreUsuario);
        }

        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            correo = correo.Trim().ToLower();

            return await _usuarioRepository.ExisteCorreoAsync(correo);
        }

        public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario)
        {
            nombreUsuario = nombreUsuario.Trim().ToLower();

            return await _usuarioRepository.ExisteNombreUsuarioAsync(nombreUsuario);
        }

        public async Task CrearAsync(Usuario usuario, string claveInicial)
        {
            usuario.nombreCompleto = usuario.nombreCompleto.Trim();
            usuario.nombreUsuario = usuario.nombreUsuario.Trim().ToLower();
            usuario.correo = usuario.correo.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(usuario.nombreCompleto))
            {
                throw new InvalidOperationException("El nombre completo es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(usuario.nombreUsuario))
            {
                throw new InvalidOperationException("El nombre de usuario es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(claveInicial))
            {
                throw new InvalidOperationException("La contraseña inicial es obligatoria.");
            }

            if (usuario.idRolUsuario <= 0)
            {
                throw new InvalidOperationException("Debe seleccionar un rol válido.");
            }

            if (usuario.idDelegacionUsuario <= 0)
            {
                throw new InvalidOperationException("Debe seleccionar una delegación válida.");
            }

            if (await _usuarioRepository.ExisteNombreUsuarioAsync(usuario.nombreUsuario))
            {
                throw new InvalidOperationException("Ya existe un usuario con ese nombre de usuario.");
            }

            if (await _usuarioRepository.ExisteCorreoAsync(usuario.correo))
            {
                throw new InvalidOperationException("Ya existe un usuario con ese correo electrónico.");
            }

            usuario.estado = true;
            usuario.debeCambiarClave = true;
            usuario.fechaUltimoCambioClave = DateTime.UtcNow;
            usuario.VersionSeguridad = Guid.NewGuid();

            usuario.claveHash = _passwordHasher.HashPassword(usuario, claveInicial);

            await _usuarioRepository.AgregarAsync(usuario);
            await _usuarioRepository.GuardarAsync();
        }

        public async Task ActualizarAsync(Usuario usuario)
        {
            _usuarioRepository.Actualizar(usuario);

            await _usuarioRepository.GuardarAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);

            if (usuario is null)
            {
                return;
            }

            _usuarioRepository.Eliminar(usuario);

            await _usuarioRepository.GuardarAsync();
        }
    }
}