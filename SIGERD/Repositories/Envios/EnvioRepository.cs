using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.Interfaces.IRespositories.Envios;
using SIGERD.Models.Envios;

namespace SIGERD.Repositories.Envios
{
    public class EnvioRepository : IEnvioRepository
    {
        private readonly ApplicationDbContext _context;

        public EnvioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Envio>> ObtenerTodosAsync()
        {
            return await _context.Envios
                .Include(e => e.DelegacionOrigen)
                .Include(e => e.DelegacionDestino)
                .Include(e => e.Usuario)
                .Include(e => e.EstadoEnvio)
                .Include(e => e.DetallesEnvio)
                .OrderByDescending(e => e.fechaEnvio)
                .ToListAsync();
        }

        public async Task<Envio?> ObtenerPorIdAsync(int idEnvio)
        {
            return await _context.Envios
                .Include(e => e.DelegacionOrigen)
                .Include(e => e.DelegacionDestino)
                .Include(e => e.Usuario)
                .Include(e => e.EstadoEnvio)
                .Include(e => e.DetallesEnvio)
                    .ThenInclude(d => d.Articulo)
                .FirstOrDefaultAsync(e => e.idEnvio == idEnvio);
        }


        public async Task AgregarAsync(Envio envio)
        {
            await _context.AddAsync(envio);
        }

        public void Actualizar(Envio envio)
        {
            _context.Envios.Update(envio);
        }

        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> ObtenerConsecutivoDiarioAsync(DateTime fecha)
        {
            DateTime inicioDia = fecha.Date;
            DateTime finDia = fecha.AddDays(1);

            int totalEnviosDelDia = await _context.Envios
                    .CountAsync(e =>
                        e.fechaEnvio >= inicioDia &&
                        e.fechaEnvio < finDia);

            return totalEnviosDelDia + 1;
        }

    }
}
