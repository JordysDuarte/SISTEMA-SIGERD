using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.DTOs.Reportes;
using SIGERD.Interfaces.IRespositories.Reportes;
using SIGERD.Models.Envios;

namespace SIGERD.Repositories.Reportes
{
    public class ReporteEnviosRepository : IReporteEnviosRepository
    {
        private readonly ApplicationDbContext _context;

        public ReporteEnviosRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Envio>> ObtenerEnviosAsync(ReporteEnviosFiltroDto filtro)
        {
            var query = _context.Envios
                .AsNoTracking()
                .Include(e => e.DelegacionOrigen)
                .Include(e => e.DelegacionDestino)
                .Include(e => e.Usuario)
                .Include(e => e.UsuarioDespacho)
                .Include(e => e.EstadoEnvio)
                .Include(e => e.Recepcion)
                    .ThenInclude(r => r.Usuario)
                .Include(e => e.DetallesEnvio)
                .AsQueryable();

            if (!filtro.EsSuperAdministrador)
            {
                query = query.Where(e =>
                    e.idDelegacionOrigenEnvio == filtro.IdDelegacionUsuario ||
                    e.idDelegacionDestinoEnvio == filtro.IdDelegacionUsuario
                );
            }

            if (filtro.FechaInicio.HasValue)
            {
                DateTime fechaInicio = filtro.FechaInicio.Value.Date;

                query = query.Where(e => e.fechaEnvio >= fechaInicio);
            }

            if (filtro.FechaFin.HasValue)
            {
                DateTime fechaFin = filtro.FechaFin.Value.Date.AddDays(1).AddTicks(-1);

                query = query.Where(e => e.fechaEnvio <= fechaFin);
            }

            if (filtro.IdDelegacionOrigen.HasValue && filtro.IdDelegacionOrigen.Value > 0)
            {
                query = query.Where(e => e.idDelegacionOrigenEnvio == filtro.IdDelegacionOrigen.Value);
            }

            if (filtro.IdDelegacionDestino.HasValue && filtro.IdDelegacionDestino.Value > 0)
            {
                query = query.Where(e => e.idDelegacionDestinoEnvio == filtro.IdDelegacionDestino.Value);
            }

            if (filtro.IdEstadoEnvio.HasValue && filtro.IdEstadoEnvio.Value > 0)
            {
                query = query.Where(e => e.idEstadoEnvioEnvio == filtro.IdEstadoEnvio.Value);
            }

            return await query
                .OrderByDescending(e => e.fechaEnvio)
                .ToListAsync();
        }
    }
}