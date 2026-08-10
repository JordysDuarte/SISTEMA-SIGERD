using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.Interfaces.IRespositories.Inventario;
using SIGERD.Models.Inventario;

namespace SIGERD.Repositories.Inventario
{
    public class ArticuloRepository : IArticuloRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticuloRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Articulo>> ObtenerTodosAsync()
        {
            return await _context.Articulos
                .AsNoTracking()
                .Include(a => a.Categoria)
                .OrderBy(a => a.nombreArticulo)
                .ToListAsync();
        }

        public async Task<Articulo?> ObtenerPorIdAsync(int idArticulo)
        {
            return await _context.Articulos
                .Include(a => a.Categoria)
                .FirstOrDefaultAsync(a => a.idArticulo == idArticulo);
        }

        public async Task<bool> ExisteNombreAsync(string nombreArticulo, int? idArticuloExcluir = null)
        {
            string nombreNormalizado = nombreArticulo.Trim().ToLower();

            var query = _context.Articulos
                .AsNoTracking()
                .Where(a =>
                    a.nombreArticulo != null &&
                    a.nombreArticulo.Trim().ToLower() == nombreNormalizado
                );

            if (idArticuloExcluir.HasValue)
            {
                query = query.Where(a => a.idArticulo != idArticuloExcluir.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExisteCategoriaActivaAsync(int idCategoria)
        {
            return await _context.Categorias
                .AsNoTracking()
                .AnyAsync(c => c.idCategoria == idCategoria && c.estado);
        }

        public async Task AgregarAsync(Articulo articulo)
        {
            await _context.Articulos.AddAsync(articulo);
        }

        public void Actualizar(Articulo articulo)
        {
            _context.Articulos.Update(articulo);
        }

        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}