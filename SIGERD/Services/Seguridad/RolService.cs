using SIGERD.Constants.Seguridad;
using SIGERD.Interfaces.IRespositories.Seguridad;
using SIGERD.Interfaces.IServices.Seguridad;
using SIGERD.Models.Seguridad;

namespace SIGERD.Services.Seguridad
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepository;

        public RolService(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
        {
            return await _rolRepository.ObtenerTodosAsync();
        }

        public async Task<Rol?> ObtenerPorIdAsync(int idRol)
        {
            if (idRol <= 0)
            {
                return null;
            }

            return await _rolRepository.ObtenerPorIdAsync(idRol);
        }

        public async Task CrearAsync(Rol rol)
        {
            if (rol is null)
            {
                throw new InvalidOperationException("La información del rol no es válida.");
            }

            rol.nombreRol = rol.nombreRol?.Trim() ?? string.Empty;
            rol.descripcion = rol.descripcion?.Trim();

            if (string.IsNullOrWhiteSpace(rol.nombreRol))
            {
                throw new InvalidOperationException("Debe ingresar el nombre del rol.");
            }

            bool existeNombre = await _rolRepository.ExisteNombreAsync(rol.nombreRol);

            if (existeNombre)
            {
                throw new InvalidOperationException("Ya existe un rol con ese nombre.");
            }

            await _rolRepository.AgregarAsync(rol);
            await _rolRepository.GuardarAsync();
        }

        public async Task ActualizarAsync(Rol rol)
        {
            if (rol is null)
            {
                throw new InvalidOperationException("La información del rol no es válida.");
            }

            if (rol.idRol <= 0)
            {
                throw new InvalidOperationException("El identificador del rol no es válido.");
            }

            var rolActual = await _rolRepository.ObtenerPorIdAsync(rol.idRol);

            if (rolActual is null)
            {
                throw new InvalidOperationException("El rol solicitado no existe.");
            }

            if (EsRolBase(rolActual.nombreRol))
            {
                throw new InvalidOperationException("No se permite editar los roles base del sistema.");
            }

            rol.nombreRol = rol.nombreRol?.Trim() ?? string.Empty;
            rol.descripcion = rol.descripcion?.Trim();

            if (string.IsNullOrWhiteSpace(rol.nombreRol))
            {
                throw new InvalidOperationException("Debe ingresar el nombre del rol.");
            }

            bool existeNombre = await _rolRepository.ExisteNombreAsync(
                rol.nombreRol,
                rol.idRol
            );

            if (existeNombre)
            {
                throw new InvalidOperationException("Ya existe otro rol con ese nombre.");
            }

            rolActual.nombreRol = rol.nombreRol;
            rolActual.descripcion = rol.descripcion;

            _rolRepository.Actualizar(rolActual);
            await _rolRepository.GuardarAsync();
        }

        private static bool EsRolBase(string? nombreRol)
        {
            if (string.IsNullOrWhiteSpace(nombreRol))
            {
                return false;
            }

            return nombreRol.Equals(RolesSistema.SuperAdministrador, StringComparison.OrdinalIgnoreCase)
                || nombreRol.Equals(RolesSistema.Administrador, StringComparison.OrdinalIgnoreCase)
                || nombreRol.Equals(RolesSistema.Usuario, StringComparison.OrdinalIgnoreCase);
        }
    }
}