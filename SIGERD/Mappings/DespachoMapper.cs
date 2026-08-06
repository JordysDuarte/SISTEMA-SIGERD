using SIGERD.Models.Envios;
using SIGERD.ViewModels.Despachos;

namespace SIGERD.Mappings
{
    public static class DespachoMapper
    {
        public static DespachoListViewModel ToListViewModel(Envio envio)
        {
            return new DespachoListViewModel
            {
                IdEnvio = envio.idEnvio,
                CodigoEnvio = envio.codigoEnvio ?? string.Empty,
                FechaEnvio = envio.fechaEnvio,
                DelegacionOrigen = envio.DelegacionOrigen?.nombreDelegacion ?? "Sin origen",
                DelegacionDestino = envio.DelegacionDestino?.nombreDelegacion ?? "Sin destino",
                UsuarioEnvio = envio.Usuario?.nombreCompleto ?? "Sin usuario",
                EstadoEnvio = envio.EstadoEnvio?.nombreEstadoEnvio ?? "Sin estado",
                TotalArticulos = envio.DetallesEnvio?.Count ?? 0,
                Observaciones = envio.observaciones
            };
        }
    }
}
