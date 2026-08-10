using SIGERD.Models.Inventario;

namespace SIGERD.Interfaces.IServices.Inventario
{
    public interface IArticuloService
    {
        Task<IEnumerable<Articulo>> ObtenerTodosAsync();

        Task<Articulo?> ObtenerPorIdAsync(int idArticulo);

        Task CrearAsync(Articulo articulo);

        Task ActualizarAsync(Articulo articulo);

        Task CambiarEstadoAsync(int idArticulo, bool nuevoEstado);
    }
}