using Microsoft.AspNetCore.Mvc.Rendering;
using SIGERD.Interfaces.IServices.Common;
using SIGERD.Interfaces.IServices.Seguridad;
using SIGERD.Interfaces.IServices.Ubicacion;

namespace SIGERD.Services.Common
{
    public class SelectListService : ISelectListService
    {
        private readonly IRolService _rolService;
        private readonly IDelegacionService _delegacionService;

        public SelectListService(
            IRolService rolService,
            IDelegacionService delegacionService)
        {
            _rolService = rolService;
            _delegacionService = delegacionService;
        }

        public async Task<IEnumerable<SelectListItem>> ObtenerRolesAsync()
        {
            var roles = await _rolService.ObtenerTodosAsync();

            return roles.Select(r => new SelectListItem
            {
                Value = r.idRol.ToString(),
                Text = r.nombreRol
            });
        }

        public async Task<IEnumerable<SelectListItem>> ObtenerDelegacionesAsync()
        {
            var delegaciones = await _delegacionService.ObtenerTodasAsync();

            return delegaciones.Select(d => new SelectListItem
            {
                Value = d.idDelegacion.ToString(),
                Text = d.nombreDelegacion
            });
        }
    }
}