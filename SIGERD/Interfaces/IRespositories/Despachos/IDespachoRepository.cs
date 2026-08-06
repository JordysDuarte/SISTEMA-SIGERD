using SIGERD.Models.Envios;

namespace SIGERD.Interfaces.IRespositories.Despachos
{
    public interface IDespachoRepository 
    {
        Task<IEnumerable<Envio>> ObtenerPendientesAsync(int? idDelegacionOrigen = null);

        Task<Envio?> ObtenerPorIdAsync(int idEnvio);

        Task<int?> ObtenerIdEstadoPorNombreAsync(string nombreEstado);

        void Actualizar(Envio envio);

        Task GuardarAsync();
    }
}
