using SIGERD.DTOs.Reportes;
using SIGERD.Interfaces.IRespositories.Reportes;
using SIGERD.Interfaces.IServices.Reportes;
using SIGERD.Models.Envios;
using SIGERD.ViewModels.Reportes.Envios;

namespace SIGERD.Services.Reportes
{
    public class ReporteEnviosService : IReporteEnviosService
    {
        private readonly IReporteEnviosRepository _reporteEnviosRepository;

        public ReporteEnviosService(IReporteEnviosRepository reporteEnviosRepository)
        {
            _reporteEnviosRepository = reporteEnviosRepository;
        }

        public async Task<ReporteEnviosIndexViewModel> ObtenerReporteAsync(ReporteEnviosFiltroDto filtro)
        {
            ValidarFiltro(filtro);

            var envios = (await _reporteEnviosRepository.ObtenerEnviosAsync(filtro)).ToList();

            var resultados = envios
                .Select(ToResultadoViewModel)
                .ToList();

            return new ReporteEnviosIndexViewModel
            {
                Resultados = resultados,
                TotalEnvios = resultados.Count,
                TotalPendientes = resultados.Count(e => EsEstado(e.EstadoEnvio, "Pendiente")),
                TotalEnTransito = resultados.Count(e =>
                    EsEstado(e.EstadoEnvio, "En tránsito") ||
                    EsEstado(e.EstadoEnvio, "En transito")),
                TotalRecibidos = resultados.Count(e => EsEstado(e.EstadoEnvio, "Recibido"))
            };
        }

        private static void ValidarFiltro(ReporteEnviosFiltroDto filtro)
        {
            if (filtro is null)
            {
                throw new InvalidOperationException("La información del filtro no es válida.");
            }

            if (filtro.FechaInicio.HasValue &&
                filtro.FechaFin.HasValue &&
                filtro.FechaInicio.Value.Date > filtro.FechaFin.Value.Date)
            {
                throw new InvalidOperationException("La fecha inicio no puede ser mayor que la fecha fin.");
            }

            if (!filtro.EsSuperAdministrador && filtro.IdDelegacionUsuario <= 0)
            {
                throw new InvalidOperationException("No fue posible identificar la delegación del usuario.");
            }
        }

        private static ReporteEnviosResultadoViewModel ToResultadoViewModel(Envio envio)
        {
            return new ReporteEnviosResultadoViewModel
            {
                IdEnvio = envio.idEnvio,
                CodigoEnvio = envio.codigoEnvio ?? string.Empty,
                FechaEnvio = envio.fechaEnvio,
                DelegacionOrigen = envio.DelegacionOrigen?.nombreDelegacion ?? "Sin origen",
                DelegacionDestino = envio.DelegacionDestino?.nombreDelegacion ?? "Sin destino",
                UsuarioEnvio = envio.Usuario?.nombreCompleto ?? "Sin usuario",
                EstadoEnvio = envio.EstadoEnvio?.nombreEstadoEnvio ?? "Sin estado",
                FechaDespacho = envio.fechaDespacho,
                UsuarioDespacho = envio.UsuarioDespacho?.nombreCompleto,
                FechaRecepcion = envio.Recepcion?.fechaRecepcion,
                UsuarioRecepcion = envio.Recepcion?.Usuario?.nombreCompleto,
                TotalArticulos = envio.DetallesEnvio?.Count ?? 0
            };
        }

        private static bool EsEstado(string estadoActual, string estadoEsperado)
        {
            return estadoActual.Trim().Equals(
                estadoEsperado,
                StringComparison.OrdinalIgnoreCase
            );
        }
    }
}