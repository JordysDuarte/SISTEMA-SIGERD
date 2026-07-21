using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.Interfaces.IRespositories.Seguridad;
using SIGERD.Models.Seguridad;
using SIGERD.Repositories.Base;

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
                .Include(u => u.Delegacion)
                .OrderBy(u => u.nombreCompleto)
                .ToListAsync();
        }

        public override async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Delegacion)
                .FirstOrDefaultAsync(u => u.idUsuario == id);
        }

        public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.correo == correo);
        }

        public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Delegacion)
                .FirstOrDefaultAsync(u => u.nombreUsuario == nombreUsuario);
        }

        public async Task<bool> ExisteCorreoAsync(string correo, int? idUsuarioExcluir = null)
        {
            correo = correo.Trim().ToLower();

            return await _context.Usuarios
                .AnyAsync(u => u.correo == correo &&
                            (!idUsuarioExcluir.HasValue || u.idUsuario != idUsuarioExcluir.Value)
                            );
        }

        public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, int? idUsuarioExcluir = null)
        {
            nombreUsuario = nombreUsuario.Trim().ToLower();

            return await _context.Usuarios
                .AnyAsync(u => u.nombreUsuario == nombreUsuario &&
                            (!idUsuarioExcluir.HasValue || u.idUsuario != idUsuarioExcluir.Value)
                            );
        }
    }
}