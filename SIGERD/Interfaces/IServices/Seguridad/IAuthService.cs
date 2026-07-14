using SIGERD.DTOs.Auth;

namespace SIGERD.Interfaces.IServices.Seguridad
{
    public interface IAuthService
    {
        Task<ResultadoAutenticacionDto> AutenticarAsync(
            string nombreUsuario,
            string clave
         );

        Task CambiarClaveInicialAsync(
            int idUsuario,
            string nuevaClave
        );
    }
}
