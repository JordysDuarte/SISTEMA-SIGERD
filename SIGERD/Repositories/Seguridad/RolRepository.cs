using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.Interfaces.IRespositories.Seguridad;
using SIGERD.Models.Seguridad;

namespace SIGERD.Repositories.Seguridad
{
    public class RolRepository : IRolRepository
    {
        private readonly ApplicationDbContext _context;

        public RolRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
        {
            return await _context.Roles
                .AsNoTracking()
                .OrderBy(r => r.idRol)
                .ToListAsync();
        }

        public async Task<Rol?> ObtenerPorIdAsync(int idRol)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.idRol == idRol);
        }

        public async Task<Rol?> ObtenerPorNombreAsync(string nombreRol)
        {
            string nombreNormalizado = nombreRol.Trim().ToLower();

            return await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r =>
                    r.nombreRol != null &&
                    r.nombreRol.Trim().ToLower() == nombreNormalizado
                );
        }

        public async Task<bool> ExisteNombreAsync(string nombreRol, int? idRolExcluir = null)
        {
            string nombreNormalizado = nombreRol.Trim().ToLower();

            var query = _context.Roles
                .AsNoTracking()
                .Where(r =>
                    r.nombreRol != null &&
                    r.nombreRol.Trim().ToLower() == nombreNormalizado
                );

            if (idRolExcluir.HasValue)
            {
                query = query.Where(r => r.idRol != idRolExcluir.Value);
            }

            return await query.AnyAsync();
        }

        public async Task AgregarAsync(Rol rol)
        {
            await _context.Roles.AddAsync(rol);
        }

        public void Actualizar(Rol rol)
        {
            _context.Roles.Update(rol);
        }

        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}