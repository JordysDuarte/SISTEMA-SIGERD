using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.Interfaces.IRespositories.Recepciones;
using SIGERD.Models.Envios;
using SIGERD.Models.Recepciones;

namespace SIGERD.Repositories.Recepciones
{
    public class RecepcionRepository : IRecepcionRepository
    {
        private readonly ApplicationDbContext _context;

        public RecepcionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private IQueryable<Envio> ConsultaEnviosBase()
        {
            return _context.Envios
                .Include(e => e.DelegacionOrigen)
                .Include(e => e.DelegacionDestino)
                .Include(e => e.Usuario)
                .Include(e => e.UsuarioDespacho)
                .Include(e => e.EstadoEnvio)
                .Include(e => e.Recepcion)
                .Include(e => e.DetallesEnvio)
                    .ThenInclude(d => d.Articulo);
        }

        public async Task<IEnumerable<Envio>> ObtenerEnviosEnTransitoAsync(int? idDelegacionDestino = null)
        {
            var query = ConsultaEnviosBase()
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

            return await query
                .OrderByDescending(e => e.fechaDespacho ?? e.fechaEnvio)
                .ToListAsync();
        }

        public async Task<Envio?> ObtenerEnvioPorIdAsync(int idEnvio)
        {
            return await ConsultaEnviosBase()
                .FirstOrDefaultAsync(e => e.idEnvio == idEnvio);
        }

        public async Task<Recepcion?> ObtenerPorIdAsync(int idRecepcion)
        {
            return await _context.Recepciones
                .Include(r => r.Usuario)
                .Include(r => r.Envio)
                    .ThenInclude(e => e!.DelegacionOrigen)
                .Include(r => r.Envio)
                    .ThenInclude(e => e!.DelegacionDestino)
                .Include(r => r.Envio)
                    .ThenInclude(e => e!.Usuario)
                .Include(r => r.Envio)
                    .ThenInclude(e => e!.UsuarioDespacho)
                .Include(r => r.Envio)
                    .ThenInclude(e => e!.EstadoEnvio)
                .Include(r => r.Envio)
                    .ThenInclude(e => e!.DetallesEnvio)
                        .ThenInclude(d => d.Articulo)
                .FirstOrDefaultAsync(r => r.idRecepcion == idRecepcion);
        }

        public async Task<Recepcion?> ObtenerPorEnvioAsync(int idEnvio)
        {
            return await _context.Recepciones
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.idEnvioRecepcion == idEnvio);
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

        public async Task AgregarAsync(Recepcion recepcion)
        {
            await _context.Recepciones.AddAsync(recepcion);
        }

        public void ActualizarEnvio(Envio envio)
        {
            _context.Envios.Update(envio);
        }

        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}