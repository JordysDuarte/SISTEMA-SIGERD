using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.Interfaces.IRespositories.Inventario;
using SIGERD.Models.Inventario;

namespace SIGERD.Repositories.Inventario
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> ObtenerTodosAsync()
        {
            return await _context.Categorias
                .AsNoTracking()
                .Include(c => c.Articulos)
                .OrderBy(c => c.nombreCategoria)
                .ToListAsync();
        }

        public async Task<Categoria?> ObtenerPorIdAsync(int idCategoria)
        {
            return await _context.Categorias
                .Include(c => c.Articulos)
                .FirstOrDefaultAsync(c => c.idCategoria == idCategoria);
        }

        public async Task<bool> ExisteNombreAsync(string nombreCategoria, int? idCategoriaExcluir = null)
        {
            string nombreNormalizado = nombreCategoria.Trim().ToLower();

            var query = _context.Categorias
                .AsNoTracking()
                .Where(c =>
                    c.nombreCategoria != null &&
                    c.nombreCategoria.Trim().ToLower() == nombreNormalizado
                );

            if (idCategoriaExcluir.HasValue)
            {
                query = query.Where(c => c.idCategoria != idCategoriaExcluir.Value);
            }

            return await query.AnyAsync();
        }

        public async Task AgregarAsync(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
        }

        public void Actualizar(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
        }

        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}