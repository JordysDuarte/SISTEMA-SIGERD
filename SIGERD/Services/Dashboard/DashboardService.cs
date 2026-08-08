using SIGERD.Interfaces.IRespositories.Dashboard;
using SIGERD.Interfaces.IServices.Dashboard;
using SIGERD.Models.Envios;
using SIGERD.ViewModels.Dashboard;

namespace SIGERD.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardViewModel> ObtenerDashboardAsync(
            int idDelegacionUsuario,
            string nombreUsuario,
            string nombreDelegacion,
            bool esSuperAdministrador)
        {
            if (esSuperAdministrador)
            {
                return await ObtenerDashboardSuperAdministradorAsync(
                    nombreUsuario,
                    nombreDelegacion
                );
            }

            return await ObtenerDashboardPorDelegacionAsync(
                idDelegacionUsuario,
                nombreUsuario,
                nombreDelegacion
            );
        }

        private async Task<DashboardViewModel> ObtenerDashboardSuperAdministradorAsync(
            string nombreUsuario,
            string nombreDelegacion)
        {
            var ultimosEnvios = await _dashboardRepository.ObtenerUltimosEnviosAsync(8);

            return new DashboardViewModel
            {
                EsSuperAdministrador = true,
                NombreUsuario = nombreUsuario,
                NombreDelegacion = nombreDelegacion,
                TituloContexto = "Vista general del sistema",

                TotalUsuarios = await _dashboardRepository.ContarUsuariosAsync(),
                TotalArticulos = await _dashboardRepository.ContarArticulosAsync(),
                TotalEnvios = await _dashboardRepository.ContarEnviosAsync(),
                TotalRecepciones = await _dashboardRepository.ContarRecepcionesAsync(),

                PendientesDespacho = await _dashboardRepository.ContarPendientesDespachoAsync(),
                PendientesRecepcion = await _dashboardRepository.ContarPendientesRecepcionAsync(),
                EnviosEnTransito = await _dashboardRepository.ContarEnviosEnTransitoAsync(),
                EnviosRecibidos = await _dashboardRepository.ContarEnviosRecibidosAsync(),

                UltimosEnvios = ultimosEnvios
                    .Select(ToDashboardEnvioViewModel)
                    .ToList()
            };
        }

        private async Task<DashboardViewModel> ObtenerDashboardPorDelegacionAsync(
            int idDelegacionUsuario,
            string nombreUsuario,
            string nombreDelegacion)
        {
            if (idDelegacionUsuario <= 0)
            {
                return new DashboardViewModel
                {
                    EsSuperAdministrador = false,
                    NombreUsuario = nombreUsuario,
                    NombreDelegacion = nombreDelegacion,
                    TituloContexto = "No fue posible identificar la delegación del usuario"
                };
            }

            var ultimosEnvios = await _dashboardRepository.ObtenerUltimosEnviosRelacionadosAsync(
                idDelegacionUsuario,
                8
            );

            return new DashboardViewModel
            {
                EsSuperAdministrador = false,
                NombreUsuario = nombreUsuario,
                NombreDelegacion = nombreDelegacion,
                TituloContexto = $"Resumen de {nombreDelegacion}",

                EnviosEnviados = await _dashboardRepository.ContarEnviosEnviadosPorDelegacionAsync(idDelegacionUsuario),
                EnviosDestinados = await _dashboardRepository.ContarEnviosDestinadosADelegacionAsync(idDelegacionUsuario),

                PendientesDespacho = await _dashboardRepository.ContarPendientesDespachoAsync(idDelegacionUsuario),
                PendientesRecepcion = await _dashboardRepository.ContarPendientesRecepcionAsync(idDelegacionUsuario),

                EnviosEnTransito = await _dashboardRepository.ContarEnviosEnTransitoAsync(idDelegacionUsuario),
                EnviosRecibidos = await _dashboardRepository.ContarEnviosRecibidosAsync(idDelegacionUsuario),

                UltimosEnvios = ultimosEnvios
                    .Select(ToDashboardEnvioViewModel)
                    .ToList()
            };
        }

        private static DashboardEnvioViewModel ToDashboardEnvioViewModel(Envio envio)
        {
            return new DashboardEnvioViewModel
            {
                IdEnvio = envio.idEnvio,
                CodigoEnvio = envio.codigoEnvio ?? string.Empty,
                FechaEnvio = envio.fechaEnvio,
                DelegacionOrigen = envio.DelegacionOrigen?.nombreDelegacion ?? "Sin origen",
                DelegacionDestino = envio.DelegacionDestino?.nombreDelegacion ?? "Sin destino",
                EstadoEnvio = envio.EstadoEnvio?.nombreEstadoEnvio ?? "Sin estado"
            };
        }
    }
}