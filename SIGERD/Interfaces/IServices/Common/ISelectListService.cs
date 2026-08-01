using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIGERD.Interfaces.IServices.Common
{
    public interface ISelectListService
    {
        Task<IEnumerable<SelectListItem>> ObtenerRolesAsync();

        Task<IEnumerable<SelectListItem>> ObtenerDelegacionesAsync();

        Task<IEnumerable<SelectListItem>> ObtenerArticulosAsync();

        Task<IEnumerable<SelectListItem>> ObtenerEstadosEnvioAsync();
    }
}
