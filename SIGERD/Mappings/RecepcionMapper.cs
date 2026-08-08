using SIGERD.Models.Envios;
using SIGERD.Models.Recepciones;
using SIGERD.ViewModels.Recepciones;

namespace SIGERD.Mappings
{
    public static class RecepcionMapper
    {
        public static RecepcionEnvioListViewModel ToListViewModel(Envio envio)
        {
            return new RecepcionEnvioListViewModel
            {
                IdEnvio = envio.idEnvio,
                CodigoEnvio = envio.codigoEnvio ?? string.Empty,
                FechaEnvio = envio.fechaEnvio,
                FechaDespacho = envio.fechaDespacho,
                DelegacionOrigen = envio.DelegacionOrigen?.nombreDelegacion ?? "Sin origen",
                DelegacionDestino = envio.DelegacionDestino?.nombreDelegacion ?? "Sin destino",
                UsuarioEnvio = envio.Usuario?.nombreCompleto ?? "Sin usuario",
                UsuarioDespacho = envio.UsuarioDespacho?.nombreCompleto,
                EstadoEnvio = envio.EstadoEnvio?.nombreEstadoEnvio ?? "Sin estado",
                TotalArticulos = envio.DetallesEnvio?.Count ?? 0
            };
        }

        public static RecepcionConfirmViewModel ToConfirmViewModel(Envio envio)
        {
            return new RecepcionConfirmViewModel
            {
                IdEnvio = envio.idEnvio,
                CodigoEnvio = envio.codigoEnvio ?? string.Empty,
                FechaEnvio = envio.fechaEnvio,
                FechaDespacho = envio.fechaDespacho,
                DelegacionOrigen = envio.DelegacionOrigen?.nombreDelegacion ?? "Sin origen",
                DelegacionDestino = envio.DelegacionDestino?.nombreDelegacion ?? "Sin destino",
                UsuarioEnvio = envio.Usuario?.nombreCompleto ?? "Sin usuario",
                UsuarioDespacho = envio.UsuarioDespacho?.nombreCompleto,
                EstadoEnvio = envio.EstadoEnvio?.nombreEstadoEnvio ?? "Sin estado",
                Detalles = envio.DetallesEnvio
                    .Select(d => new RecepcionDetalleViewModel
                    {
                        Articulo = d.Articulo?.nombreArticulo ?? "Sin artículo",
                        Cantidad = d.cantidad,
                        ObservacionesDetalle = d.observacionesDetalleEnvio
                    })
                    .ToList()
            };
        }

        public static RecepcionDetailsViewModel ToDetailsViewModel(Recepcion recepcion)
        {
            var envio = recepcion.Envio;

            return new RecepcionDetailsViewModel
            {
                IdRecepcion = recepcion.idRecepcion,
                IdEnvio = envio?.idEnvio ?? 0,
                CodigoEnvio = envio?.codigoEnvio ?? string.Empty,
                FechaEnvio = envio?.fechaEnvio ?? DateTime.MinValue,
                FechaDespacho = envio?.fechaDespacho,
                FechaRecepcion = recepcion.fechaRecepcion,
                DelegacionOrigen = envio?.DelegacionOrigen?.nombreDelegacion ?? "Sin origen",
                DelegacionDestino = envio?.DelegacionDestino?.nombreDelegacion ?? "Sin destino",
                UsuarioEnvio = envio?.Usuario?.nombreCompleto ?? "Sin usuario",
                UsuarioDespacho = envio?.UsuarioDespacho?.nombreCompleto,
                UsuarioRecepcion = recepcion.Usuario?.nombreCompleto ?? "Sin usuario",
                EstadoEnvio = envio?.EstadoEnvio?.nombreEstadoEnvio ?? "Sin estado",
                Observaciones = recepcion.observaciones,
                Detalles = envio?.DetallesEnvio
                    .Select(d => new RecepcionDetalleViewModel
                    {
                        Articulo = d.Articulo?.nombreArticulo ?? "Sin artículo",
                        Cantidad = d.cantidad,
                        ObservacionesDetalle = d.observacionesDetalleEnvio
                    })
                    .ToList() ?? new List<RecepcionDetalleViewModel>()
            };
        }
    }
}