using SIGERD.Data;
using SIGERD.Interfaces.IRespositories.Despachos;
using SIGERD.Models.Envios;
using Microsoft.EntityFrameworkCore;

namespace SIGERD.Repositories.Despachos
{
    public class DespachoRepository : IDespachoRepository
    {
        private readonly ApplicationDbContext _context;

        public DespachoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Envio>> ObtenerPendientesAsync(int? idDelegacionOrigen = null)
        {
            var query = _context.Envios
                .Include(e => e.DelegacionOrigen)
                .Include(e => e.DelegacionDestino)
                .Include(e => e.Usuario)
                .Include(e => e.EstadoEnvio)
                .Include(e => e.DetallesEnvio)
                .Where(e =>
                    e.EstadoEnvio != null &&
                    e.EstadoEnvio.nombreEstadoEnvio != null &&
                    e.EstadoEnvio.nombreEstadoEnvio.Trim().ToLower() == "pendiente"
                );

            if (idDelegacionOrigen.HasValue)
            {
                query = query.Where(e => e.idDelegacionOrigenEnvio == idDelegacionOrigen.Value);
            }

            return await query
                .OrderByDescending(e => e.fechaEnvio)
                .ToListAsync();
        }

        public async Task<Envio?> ObtenerPorIdAsync(int idEnvio)
        {
            return await _context.Envios
                .Include(e => e.DelegacionOrigen)
                .Include(e => e.DelegacionDestino)
                .Include(e => e.Usuario)
                .Include(e => e.UsuarioDespacho)
                .Include(e => e.EstadoEnvio)
                .Include(e => e.DetallesEnvio)
                    .ThenInclude(d => d.Articulo)
                .FirstOrDefaultAsync(e => e.idEnvio == idEnvio);
        }

        public async Task<int?> ObtenerIdEstadoPorNombreAsync(string nombreEstado)
        {
            var estado = await _context.EstadoEnvios
                .AsNoTracking()
                .FirstOrDefaultAsync(e =>
                    e.nombreEstadoEnvio != null &&
                    e.nombreEstadoEnvio.Trim().ToLower() == nombreEstado.Trim().ToLower()
                );

            return estado?.idEstadoEnvio;
        }

        public void Actualizar(Envio envio)
        {
            _context.Envios.Update(envio);
        }

        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
