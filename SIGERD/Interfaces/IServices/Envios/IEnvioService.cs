using SIGERD.Models.Envios;

namespace SIGERD.Interfaces.IServices.Envios
{
    public interface IEnvioService
    {
        Task<IEnumerable<Envio>> ObtenerTodosAsync();

        Task<IEnumerable<Envio>> ObtenerPorVistaAsync(
            string? tipoVista,
            int idDelegacionUsuario,
            bool esSuperAdministrador
        );

        Task<Envio?> ObtenerPorIdValidadoAsync(
            int idEnvio,
            int idDelegacionUsuario,
            bool esSuperAdministrador
        );

        Task<Envio?> ObtenerPorIdAsync(int idEnvio);

        Task<int> CrearAsync(
            Envio envio,
            int idDelegacionUsuario,
            bool esSuperAdministrador                           
        );
    }
}
