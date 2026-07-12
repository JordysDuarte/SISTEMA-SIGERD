using SIGERD.Interfaces.IRespositories.Seguridad;
using SIGERD.Models.Seguridad;
using SIGERD.Repositories.Base;
using SIGERD.Data;
using Microsoft.EntityFrameworkCore;

namespace SIGERD.Repositories.Seguridad
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();
        }

        public override async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.idUsuario == id);
        }

        public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.correo == correo);
        }

        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            return await _dbSet
                .AnyAsync(u => u.correo == correo);
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string correo, string clave)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.correo == correo && u.clave == clave && u.estado);
        }
    }
}
