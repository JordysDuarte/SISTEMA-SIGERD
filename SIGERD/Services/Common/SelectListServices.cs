using Microsoft.AspNetCore.Mvc.Rendering;
using SIGERD.Interfaces.IServices.Seguridad;
using SIGERD.Interfaces.IServices.Common;

namespace SIGERD.Services.Common
{
    public class SelectListService : ISelectListService
    {
        private readonly IRolService _rolService;

        public SelectListService(IRolService rolService)
        {
            _rolService = rolService;   
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
    }
}
