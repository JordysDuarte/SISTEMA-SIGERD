using SIGERD.ViewModels.Envios.Envios;
using SIGERD.Models.Envios;

namespace SIGERD.Mappings
{
    public static class EnvioMapper
    {
        public static EnvioListViewModel ToListViewModel(Envio envio)
        {
            return new EnvioListViewModel
            {
                IdEnvio = envio.idEnvio,
                CodigoEnvio = envio.codigoEnvio ?? string.Empty,
                FechaEnvio = envio.fechaEnvio,
                DelegacionOrigen = envio.DelegacionOrigen?.nombreDelegacion ?? "Sin origen",
                DelegacionDestino = envio.DelegacionDestino?.nombreDelegacion ?? "Sin destino",
                UsuarioEnvio = envio.Usuario?.nombreCompleto ?? "Sin usuario",
                EstadoEnvio = envio.EstadoEnvio?.nombreEstadoEnvio ?? "sin estado",
                TotalArticulos = envio.DetallesEnvio?.Count ?? 0
            };
        }



        public static EnvioDetailsViewModel ToDetailsViewModel(Envio envio)
        {
            return new EnvioDetailsViewModel
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
                Observaciones = envio.observaciones,
                Detalles = envio.DetallesEnvio
                    .Select(detalle => new DetalleEnvioDetailsViewModel
                    {
                        IdDetalleEnvio = detalle.idDetalle,
                        Articulo = detalle.Articulo?.nombreArticulo ?? "Sin artículo",
                        Cantidad = detalle.cantidad,
                        ObservacionesDetalles = detalle.observacionesDetalleEnvio
                    })
                    .ToList()
            };
        }


        public static EnvioEditViewModel ToEditViewModel(Envio envio)
        {
            return new EnvioEditViewModel
            {
                IdEnvio = envio.idEnvio,
                CodigoEnvio = envio.codigoEnvio ?? string.Empty,
                FechaEnvio = envio.fechaEnvio,
                EstadoEnvio = envio.EstadoEnvio?.nombreEstadoEnvio ?? "Sin estado",
                IdDelegacionOrigen = envio.idDelegacionOrigenEnvio,
                IdDelegacionDestino = envio.idDelegacionDestinoEnvio,
                Observaciones = envio.observaciones,
                Detalles = envio.DetallesEnvio
                    .Select(detalle => new DetalleEnvioEditViewModel
                    {
                        IdDetalleEnvio = detalle.idEnvioDetalleEnvio,
                        IdArticulo = detalle.idArticuloDetalleEnvio,
                        Cantidad = detalle.cantidad,
                        ObservacionesDetalles = detalle.observacionesDetalleEnvio
                    })
                    .ToList()
            };
        }


        public static Envio ToEntity(
            EnvioCreateViewModel model,
            int idUsuarioEnvio)
        {
            return new Envio
            {
                idDelegacionOrigenEnvio = model.IdDelegacionOrigen,
                idDelegacionDestinoEnvio = model.IdDelegacionDestino,
                idUsuarioEnvio = idUsuarioEnvio,
                observaciones = model.Observaciones?.Trim(),
                DetallesEnvio = model.Detalles
                    .Where(d =>
                        d.IdArticulo.HasValue &&
                        d.IdArticulo.Value > 0 &&
                        d.Cantidad.HasValue &&
                        d.Cantidad.Value > 0)
                    .Select(d => new DetalleEnvio
                    {
                        idArticuloDetalleEnvio = d.IdArticulo!.Value,
                        cantidad = d.Cantidad!.Value,
                        observacionesDetalleEnvio = d.ObservacionesDetalle?.Trim()
                    })
                    .ToList()
            };
        }


        public static Envio ToEntity(EnvioEditViewModel model)
        {
            return new Envio
            {
                idEnvio = model.IdEnvio,
                idDelegacionOrigenEnvio = model.IdDelegacionOrigen,
                idDelegacionDestinoEnvio = model.IdDelegacionDestino,
                observaciones = model.Observaciones?.Trim(),
                DetallesEnvio = model.Detalles
                    .Where(d =>
                        d.IdArticulo.HasValue &&
                        d.IdArticulo.Value > 0 &&
                        d.Cantidad.HasValue &&
                        d.Cantidad.Value > 0)
                    .Select(d => new DetalleEnvio
                    {
                        idEnvioDetalleEnvio = d.IdDetalleEnvio ?? 0,
                        idArticuloDetalleEnvio = d.IdArticulo!.Value,
                        cantidad = d.Cantidad!.Value,
                        observacionesDetalleEnvio = d.ObservacionesDetalles?.Trim()
                    })
                    .ToList()
            };
        }
    }
}
