using Microsoft.IdentityModel.Tokens;
using SIGERD.Interfaces.IRespositories.Envios;
using SIGERD.Interfaces.IServices.Envios;
using SIGERD.Models.Envios;

namespace SIGERD.Services.Envios
{
    public class EnvioService : IEnvioService
    {
        private readonly IEnvioRepository _envioRepository;

        public EnvioService(IEnvioRepository envioRepository)
        {
            _envioRepository = envioRepository;
        }

        public async Task<IEnumerable<Envio>> ObtenerTodosAsync()
        {
            return await _envioRepository.ObtenerTodosAsync();
        }

        public async Task<Envio?> ObtenerPorIdAsync(int idEnvio)
        {
            if (idEnvio <= 0)
            {
                return null;
            }

            return await _envioRepository.ObtenerPorIdAsync(idEnvio);
        }

        public async Task<int> CrearAsync(Envio envio)
        {
            if (envio.idDelegacionOrigenEnvio <= 0)
            {
                throw new InvalidOperationException("Debe seleccionar la delegación origen.");
            }

            if (envio.idDelegacionDestinoEnvio <= 0)
            {
                throw new InvalidOperationException("Debe seleccionar la delegación destino.");
            }

            if (envio.idDelegacionOrigenEnvio == envio.idDelegacionDestinoEnvio)
            {
                throw new InvalidOperationException("La delegación origen y destino no pueden ser la misma.");
            }

            if (!await _envioRepository.ExisteDelegacionAsync(envio.idDelegacionOrigenEnvio))
            {
                throw new InvalidOperationException("La delegación origen seleccionada no existe.");
            }

            if (!await _envioRepository.ExisteDelegacionAsync(envio.idDelegacionDestinoEnvio))
            {
                throw new InvalidOperationException("La delegación destino seleccionada no existe.");
            }

            if (envio.idUsuarioEnvio <= 0)
            {
                throw new InvalidOperationException("No fue posible identificar al usuario que registra el envío.");
            }

            if (envio.DetallesEnvio == null || !envio.DetallesEnvio.Any())
            {
                throw new InvalidOperationException("Debe agregar al menos un artículo al envío.");
            }

            var articulosProcesados = new HashSet<int>();

            foreach (var detalle in envio.DetallesEnvio)
            {
                if (detalle.idArticuloDetalleEnvio <= 0)
                {
                    throw new InvalidOperationException("Todos los detalles deben tener un artículo válido.");
                }

                if (detalle.cantidad <= 0)
                {
                    throw new InvalidOperationException("La cantidad de cada artículo debe ser mayor que cero.");
                }

                if (!await _envioRepository.ExisteArticuloAsync(detalle.idArticuloDetalleEnvio))
                {
                    throw new InvalidOperationException($"El artículo con identificador {detalle.idArticuloDetalleEnvio} no existe.");
                }

                if (!articulosProcesados.Add(detalle.idArticuloDetalleEnvio))
                {
                    throw new InvalidOperationException("No se permite repetir el mismo artículo en el mismo envío.");
                }
            }

            int? idEstadoInicial = await _envioRepository.ObtenerIdEstadoInicialAsync();

            if (!idEstadoInicial.HasValue)
            {
                throw new InvalidOperationException("No existe un estado inicial configurado para el envío.");
            }

            envio.fechaEnvio = DateTime.Now;
            envio.codigoEnvio = await GenerarCodigoAsync(envio.fechaEnvio);
            envio.idEstadoEnvioEnvio = idEstadoInicial.Value;

            await _envioRepository.AgregarAsync(envio);
            await _envioRepository.GuardarAsync();

            return envio.idEnvio;
        }

        private async Task<string> GenerarCodigoAsync(DateTime fecha)
        {
            int consecutivo = await _envioRepository.ObtenerConsecutivoDiarioAsync(fecha);
            return $"ENV-{fecha:yyyyMMdd}-{consecutivo:0000}";
        }

    }
}
