using SIGERD.Models.Envios;
using SIGERD.Models.Recepciones;

namespace SIGERD.Interfaces.IServices.Recepciones
{
    public interface IRecepcionService
    {
        Task<IEnumerable<Envio>> ObtenerPendientesRecepcionAsync(
            int idDelegacionUsuario,
            bool esSuperAdministrador
        );

        Task<Envio?> ObtenerEnvioParaConfirmarAsync(
            int idEnvio,
            int idDelegacionUsuario,
            bool esSuperAdministrador
        );

        Task<int> ConfirmarRecepcionAsync(
            int idEnvio,
            int idUsuarioRecepcion,
            int idDelegacionUsuario,
            bool esSuperAdministrador,
            string? observaciones
        );

        Task<Recepcion?> ObtenerPorIdValidadoAsync(
            int idRecepcion,
            int idDelegacionUsuario,
            bool esSuperAdministrador
        );
    }
}