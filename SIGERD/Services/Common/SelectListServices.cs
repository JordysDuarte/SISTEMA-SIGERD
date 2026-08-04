using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.Interfaces.IServices.Common;
using SIGERD.Interfaces.IServices.Seguridad;
using SIGERD.Interfaces.IServices.Ubicacion;

namespace SIGERD.Services.Common
{
    public class SelectListService : ISelectListService
    {
        private readonly IRolService _rolService;
        private readonly IDelegacionService _delegacionService;
        private readonly ApplicationDbContext _context;

        public SelectListService(
            IRolService rolService,
            IDelegacionService delegacionService,
            ApplicationDbContext context)
        {
            _rolService = rolService;
            _delegacionService = delegacionService;
            _context = context;
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


        public async Task<IEnumerable<SelectListItem>> ObtenerArticulosAsync()
        {
            return await _context.Articulos
                .OrderBy(a => a.nombreArticulo)
                .Select(a => new SelectListItem
                {
                    Value = a.idArticulo.ToString(),
                    Text = a.nombreArticulo ?? "Sin nombre"
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> ObtenerEstadosEnvioAsync()
        {
            return await _context.EstadoEnvios
                .OrderBy(e => e.nombreEstadoEnvio)
                .Select(e => new SelectListItem
                {
                    Value = e.idEstadoEnvio.ToString(),
                    Text = e.nombreEstadoEnvio ?? "Sin estado"
                })
                .ToListAsync();
        }


    }
}