using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

        private IQueryable<Envio> ConsultaEnviosBase()
        {
            return _context.Envios
                .Include(e => e.DelegacionOrigen)
                .Include(e => e.DelegacionDestino)
                .Include(e => e.Usuario)
                .Include(e => e.UsuarioDespacho)
                .Include(e => e.EstadoEnvio)
                .Include(e => e.DetallesEnvio)
                    .ThenInclude(d => d.Articulo);
        }

        public async Task<IEnumerable<Envio>> ObtenerTodosAsync()
        {
            return await ConsultaEnviosBase()
                .OrderByDescending(e => e.fechaEnvio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Envio>> ObtenerPorDelegacionOrigenAsync(int idDelegacion)
        {
            return await ConsultaEnviosBase()
                .Where(e => e.idDelegacionOrigenEnvio == idDelegacion)
                .OrderByDescending(e => e.fechaEnvio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Envio>> ObtenerPorDelegacionDestinoAsync(int idDelegacion)
        {
            return await ConsultaEnviosBase()
                .Where(e => e.idDelegacionDestinoEnvio == idDelegacion)
                .OrderByDescending(e => e.fechaEnvio)
                .ToListAsync();
        }

        public async Task<Envio?> ObtenerPorIdAsync(int idEnvio)
        {
            return await ConsultaEnviosBase()
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


        public async Task<int?> ObtenerIdEstadoInicialAsync()
        {
            var estado = await _context.EstadoEnvios
                .AsNoTracking()
                .FirstOrDefaultAsync(e =>
                    e.nombreEstadoEnvio != null &&
                    e.nombreEstadoEnvio.Trim().ToLower() == "pendiente"
                );

            if (estado is null)
            {
                estado = await _context.EstadoEnvios
                    .AsNoTracking()
                    .OrderBy(e => e.idEstadoEnvio)
                    .FirstOrDefaultAsync();
            }

            return estado?.idEstadoEnvio;
        }


        public async Task<bool> ExisteDelegacionAsync(int idDelegacion)
        {
            return await _context.Delegaciones
                .AnyAsync(d => d.idDelegacion == idDelegacion);
        }


        public async Task<bool> ExisteArticuloAsync(int idArticulo)
        {
            return await _context.Articulos
                .AnyAsync(a => a.idArticulo == idArticulo);
        }

    }
}
