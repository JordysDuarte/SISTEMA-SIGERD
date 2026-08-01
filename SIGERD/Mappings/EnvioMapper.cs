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
                IdeEnvio = envio.idEnvio,
                CodigoEnvio = envio.codigoEnvio ?? string.Empty,
                FechaEnvio = envio.fechaEnvio,
                DelegacionOrigen = envio.DelegacionOrigen?.nombreDelegacion ?? "Sin origen",
                DelegacionDestino = envio.DelegacionDestino?.nombreDelegacion ?? "Sin destino",
                UsuarioEnvio = envio.Usuario?.nombreCompleto ?? "Sin usuario",
                EstadoEnvio = envio.EstadoEnvio?.nombreEstadoEnvio ?? "Sin estado",
                Observaciones = envio.observaciones,
                Detalles = envio.DetallesEnvio
                    .Select(detalle => new DetalleEnvioDetailsViewModel
                    {
                        IdDetalleEnvio = detalle.idDetalleEnvio,
                        Articulo = detalle.Articulo?.nombreArticulo ?? "Sin artículo",
                        Cantidad = detalle.cantidad
                    })
                    .ToList()
            };
        }


        public static Envio ToEntity(
            EnvioCreateViewModel model,
            int idUsuarioEnvio,
            int idEstadoInicial,
            string codigoEnvio)
        {
            return new Envio
            {
                codigoEnvio = codigoEnvio,
                fechaEnvio = DateTime.Now,
                idDelegacionOrigenEnvio = model.IdDelegacionOrigen,
                idDelegacionDestinoEnvio = model.IdDelegacionDestino,
                idUsuarioEnvio = idUsuarioEnvio,
                idEstadoEnvioEnvio = idEstadoInicial,
                observaciones = model.Observaciones?.Trim(),
                DetallesEnvio = model.Detalles
                    .Where(d => d.IdArticulo > 0 && d.Cantidad > 0)
                    .Select(d => new DetalleEnvio
                    {
                        idArticuloDetalleEnvio = d.IdArticulo,
                        cantidad = d.Cantidad,
                    })
                    .ToList()
            };
        }
    }
}
