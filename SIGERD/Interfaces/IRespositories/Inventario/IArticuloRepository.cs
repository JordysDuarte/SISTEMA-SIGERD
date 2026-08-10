using SIGERD.Models.Inventario;

namespace SIGERD.Interfaces.IRespositories.Inventario
{
    public interface IArticuloRepository
    {
        Task<IEnumerable<Articulo>> ObtenerTodosAsync();

        Task<Articulo?> ObtenerPorIdAsync(int idArticulo);

        Task<bool> ExisteNombreAsync(string nombreArticulo, int? idArticuloExcluir = null);

        Task<bool> ExisteCategoriaActivaAsync(int idCategoria);

        Task AgregarAsync(Articulo articulo);

        void Actualizar(Articulo articulo);

        Task GuardarAsync();
    }
}