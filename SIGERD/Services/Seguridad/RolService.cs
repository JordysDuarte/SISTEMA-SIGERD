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

        public async Task<Rol?> ObtenerPorIdAsync(int id)
        {
            return await _rolRepository.ObtenerPorIdAsync(id);
        }

        public async Task CrearAsync(Rol rol)
        {
            await _rolRepository.AgregarAsync(rol);
            await _rolRepository.GuardarAsync();
        }

        public async Task ActualizarAsync(Rol rol)
        {
            _rolRepository.Actualizar(rol);
            await _rolRepository.GuardarAsync();
        } 

        public async Task EliminarAsync(int id)
        {
            var rol = await _rolRepository.ObtenerPorIdAsync(id);

            if(rol != null)
            {
                _rolRepository.Eliminar(rol);
                await _rolRepository.GuardarAsync();
            }
        }
    }
}
