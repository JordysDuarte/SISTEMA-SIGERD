using SIGERD.Models.Envios;

namespace SIGERD.Interfaces.IRespositories.Envios
{
    public interface IEnvioRepository
    {
        Task<IEnumerable<Envio>> ObtenerTodosAsync();

        Task<IEnumerable<Envio>> ObtenerPorDelegacionOrigenAsync(int idDelegacion);

        Task<IEnumerable<Envio>> ObtenerPorDelegacionDestinoAsync(int idDelegacion);
        Task<Envio?> ObtenerPorIdAsync(int idEnvio);

        Task AgregarAsync(Envio envio);

        void Actualizar(Envio envio);

        Task GuardarAsync();

        Task<int> ObtenerConsecutivoDiarioAsync(DateTime fecha);

        Task<int?> ObtenerIdEstadoInicialAsync();

        Task<bool> ExisteDelegacionAsync(int idDelegacion);
        Task<bool> ExisteArticuloAsync(int idArticulo);


    }
}
