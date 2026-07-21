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
            usuario.versionSeguridad = Guid.NewGuid();

            usuario.claveHash = _passwordHasher.HashPassword(usuario, claveInicial);

            await _usuarioRepository.AgregarAsync(usuario);
            await _usuarioRepository.GuardarAsync();
        }

        public async Task ActualizarAsync(Usuario usuario)
        {
            if (usuario.idUsuario <= 0)
            {
                throw new InvalidOperationException("El identificador del usuario no es válido");
            }

            string nombreCompleto = usuario.nombreCompleto.Trim();
            string nombreUsuario = usuario.nombreUsuario.Trim().ToLower();
            string correo = usuario.correo.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(nombreCompleto))
            {
                throw new InvalidOperationException("El nombre completo es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                throw new InvalidOperationException("El nombre de usuario es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(correo))
            {
                throw new InvalidOperationException("El correo electrónico es obligatorio");
            }

            if (usuario.idRolUsuario <= 0)
            {
                throw new InvalidOperationException("Debe seleccionar un rol válido");
            }

            var usuarioActual = await _usuarioRepository.ObtenerPorIdAsync(usuario.idUsuario);

            if (usuarioActual == null)
            {
                throw new InvalidOperationException("El usuario que intenta editar no existe");
            }

            bool existeNombreUsuario = await _usuarioRepository.ExisteNombreUsuarioAsync(nombreUsuario, usuario.idUsuario);

            if (existeNombreUsuario)
            {
                throw new InvalidOperationException("Ya existe otro usuario con ese nombre de usuario.");
            }

            bool existeCorreo = await _usuarioRepository.ExisteCorreoAsync(
                correo,
                usuario.idUsuario
);

            if (existeCorreo)
            {
                throw new InvalidOperationException("Ya existe otro usuario con ese correo electrónico.");
            }

            usuarioActual.nombreCompleto = nombreCompleto;
            usuarioActual.nombreUsuario = nombreUsuario;
            usuarioActual.correo = correo;
            usuarioActual.idRolUsuario = usuario.idRolUsuario;
            usuarioActual.idDelegacionUsuario = usuario.idDelegacionUsuario;

            usuarioActual.versionSeguridad = Guid.NewGuid();

            _usuarioRepository.Actualizar(usuarioActual);
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