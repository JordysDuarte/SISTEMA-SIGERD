using SIGERD.Interfaces.IRespositories.Seguridad;
using SIGERD.Interfaces.IServices.Seguridad;
using SIGERD.Models.Seguridad;

namespace SIGERD.Services.Seguridad
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
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
            return await _usuarioRepository.ObtenerPorCorreoAsync(correo);
        }

        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            return await _usuarioRepository.ExisteCorreoAsync(correo);
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string correo, string clave)
        {
            return await _usuarioRepository.ValidarCredencialesAsync(correo, clave);
        }

        public async Task CrearAsync(Usuario usuario)
        {
            if (await _usuarioRepository.ExisteCorreoAsync(usuario.correo))
            {
                throw new InvalidOperationException("Ya existe un usuario con ese correo");
            }

            usuario.estado = true;  

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

            if (usuario != null)
            {
                _usuarioRepository.Eliminar(usuario);
                await _usuarioRepository.GuardarAsync();
            }
        }
    }
}
