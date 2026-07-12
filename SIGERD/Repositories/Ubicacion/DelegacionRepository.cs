using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.Interfaces.IRespositories.Ubicacion;
using SIGERD.Models.Ubicacion;
using SIGERD.Repositories.Base;

namespace SIGERD.Repositories.Ubicacion
{
    public class DelegacionRepository : GenericRepository<Delegacion>, IDelegacionRepository
    {
        public DelegacionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Delegacion>> ObtenerTodosAsync()
        {
            return await _context.Delegaciones
                .OrderBy(d => d.nombreDelegacion)
                .ToListAsync();
        }

        public async Task<bool> ExisteAsync(int idDelegacion)
        {
            return await _context.Delegaciones
                .AnyAsync(d => d.idDelegacion == idDelegacion);
        }
    }
}