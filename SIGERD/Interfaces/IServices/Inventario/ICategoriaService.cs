using SIGERD.Models.Inventario;

namespace SIGERD.Interfaces.IServices.Inventario
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> ObtenerTodosAsync();

        Task<Categoria?> ObtenerPorIdAsync(int idCategoria);

        Task CrearAsync(Categoria categoria);

        Task ActualizarAsync(Categoria categoria);

        Task CambiarEstadoAsync(int idCategoria, bool nuevoEstado);
    }
}