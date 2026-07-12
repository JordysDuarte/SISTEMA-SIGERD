using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.Interfaces.IRespositories.Dashboard;
using SIGERD.ViewModels.Dashboard;

namespace SIGERD.Repositories.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<DashboardViewModel> ObtenerResumenAsync()
        {
            return new DashboardViewModel
            {
                TotalUsuarios = await _context.Usuarios.CountAsync(),
                TotalArticulos = await _context.Articulos.CountAsync(),
                TotalRecepciones = await _context.Recepciones.CountAsync(),
                TotalEnvios = await _context.Envios.CountAsync()
            };
        }
    }
}
