using SIGERD.Models.Inventario;

namespace SIGERD.Interfaces.IRespositories.Inventario
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> ObtenerTodosAsync();

        Task<Categoria?> ObtenerPorIdAsync(int idCategoria);

        Task<bool> ExisteNombreAsync(string nombreCategoria, int? idCategoriaExcluir = null);

        Task AgregarAsync(Categoria categoria);

        void Actualizar(Categoria categoria);

        Task GuardarAsync();
    }
}