using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.Interfaces.IRespositories.Dashboard;
using SIGERD.Models.Envios;

namespace SIGERD.Repositories.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> ContarUsuariosAsync()
        {
            return await _context.Usuarios
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<int> ContarArticulosAsync()
        {
            return await _context.Articulos
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<int> ContarEnviosAsync()
        {
            return await _context.Envios
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<int> ContarRecepcionesAsync()
        {
            return await _context.Recepciones
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<int> ContarEnviosEnviadosPorDelegacionAsync(int idDelegacion)
        {
            return await _context.Envios
                .AsNoTracking()
                .CountAsync(e => e.idDelegacionOrigenEnvio == idDelegacion);
        }

        public async Task<int> ContarEnviosDestinadosADelegacionAsync(int idDelegacion)
        {
            return await _context.Envios
                .AsNoTracking()
                .CountAsync(e => e.idDelegacionDestinoEnvio == idDelegacion);
        }

        public async Task<int> ContarPendientesDespachoAsync(int? idDelegacionOrigen = null)
        {
            var query = _context.Envios
                .AsNoTracking()
                .Where(e =>
                    e.EstadoEnvio != null &&
                    e.EstadoEnvio.nombreEstadoEnvio != null &&
                    e.EstadoEnvio.nombreEstadoEnvio.Trim().ToLower() == "pendiente"
                );

            if (idDelegacionOrigen.HasValue)
            {
                query = query.Where(e => e.idDelegacionOrigenEnvio == idDelegacionOrigen.Value);
            }

            return await query.CountAsync();
        }

        public async Task<int> ContarPendientesRecepcionAsync(int? idDelegacionDestino = null)
        {
            var query = _context.Envios
                .AsNoTracking()
                .Where(e =>
                    e.Recepcion == null &&
                    e.EstadoEnvio != null &&
                    e.EstadoEnvio.nombreEstadoEnvio != null &&
                    (
                        e.EstadoEnvio.nombreEstadoEnvio.Trim().ToLower() == "en tránsito" ||
                        e.EstadoEnvio.nombreEstadoEnvio.Trim().ToLower() == "en transito"
                    )
                );

            if (idDelegacionDestino.HasValue)
            {
                query = query.Where(e => e.idDelegacionDestinoEnvio == idDelegacionDestino.Value);
            }

            return await query.CountAsync();
        }

        public async Task<int> ContarEnviosEnTransitoAsync(int? idDelegacionRelacionada = null)
        {
            var query = _context.Envios
                .AsNoTracking()
                .Where(e =>
                    e.EstadoEnvio != null &&
                    e.EstadoEnvio.nombreEstadoEnvio != null &&
                    (
                        e.EstadoEnvio.nombreEstadoEnvio.Trim().ToLower() == "en tránsito" ||
                        e.EstadoEnvio.nombreEstadoEnvio.Trim().ToLower() == "en transito"
                    )
                );

            if (idDelegacionRelacionada.HasValue)
            {
                query = query.Where(e =>
                    e.idDelegacionOrigenEnvio == idDelegacionRelacionada.Value ||
                    e.idDelegacionDestinoEnvio == idDelegacionRelacionada.Value
                );
            }

            return await query.CountAsync();
        }

        public async Task<int> ContarEnviosRecibidosAsync(int? idDelegacionRelacionada = null)
        {
            var query = _context.Envios
                .AsNoTracking()
                .Where(e =>
                    e.EstadoEnvio != null &&
                    e.EstadoEnvio.nombreEstadoEnvio != null &&
                    e.EstadoEnvio.nombreEstadoEnvio.Trim().ToLower() == "recibido"
                );

            if (idDelegacionRelacionada.HasValue)
            {
                query = query.Where(e =>
                    e.idDelegacionOrigenEnvio == idDelegacionRelacionada.Value ||
                    e.idDelegacionDestinoEnvio == idDelegacionRelacionada.Value
                );
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<Envio>> ObtenerUltimosEnviosAsync(int cantidad)
        {
            return await ConsultaEnviosBase()
                .OrderByDescending(e => e.fechaEnvio)
                .Take(cantidad)
                .ToListAsync();
        }

        public async Task<IEnumerable<Envio>> ObtenerUltimosEnviosRelacionadosAsync(int idDelegacion, int cantidad)
        {
            return await ConsultaEnviosBase()
                .Where(e =>
                    e.idDelegacionOrigenEnvio == idDelegacion ||
                    e.idDelegacionDestinoEnvio == idDelegacion
                )
                .OrderByDescending(e => e.fechaEnvio)
                .Take(cantidad)
                .ToListAsync();
        }

        private IQueryable<Envio> ConsultaEnviosBase()
        {
            return _context.Envios
                .AsNoTracking()
                .Include(e => e.DelegacionOrigen)
                .Include(e => e.DelegacionDestino)
                .Include(e => e.EstadoEnvio);
        }
    }
}