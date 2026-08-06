using SIGERD.Models.Envios;

namespace SIGERD.Interfaces.IServices.Despachos
{
    public interface IDespachoService
    {
        Task<IEnumerable<Envio>> ObtenerPendientesAsync(
            int idDelegacionUsuario,
            bool esSuperAdministrador
        );

        Task DespacharAsync(
            int idEnvio,
            int idUsuarioDespacho,
            int idDelegacionUsuario,
            bool esSuperAdministrador
        );
    }
}
