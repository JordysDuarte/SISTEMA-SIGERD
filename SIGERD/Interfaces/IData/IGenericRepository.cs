namespace SIGERD.Interfaces.IData
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObtenerTodosAsync();
        Task<T?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(T entidad);
        void Actualizar(T entidad);
        void Eliminar (T entidad);
        Task GuardarAsync();
    }
}
