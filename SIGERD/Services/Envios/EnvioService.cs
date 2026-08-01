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

        public async Task CrearAsync(Envio envio)
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

            if (envio.idUsuarioEnvio <= 0)
            {
                throw new InvalidOperationException("No fue posible identificar el usuario que registra el envío.");
            }

            if (envio.idEstadoEnvioEnvio <= 0)
            {
                throw new InvalidOperationException("No fue posible asignar el estado inicial del envío.");
            }

            if (envio.DetallesEnvio == null || !envio.DetallesEnvio.Any())
            {
                throw new InvalidOperationException("Debe agregar al menos un artículo al envío.");
            }

            foreach (var detalle in envio.DetallesEnvio)
            {
                if (detalle.idArticuloDetalleEnvio <= 0)
                {
                    throw new InvalidOperationException("Todos los detalles deben tener un artículo válido.");
                }

                if (detalle.cantidad <= 0)
                {
                    throw new InvalidOperationException("La cantidad de cada artículo debe ser mayor a cero.");
                }
            }

            await _envioRepository.AgregarAsync(envio);
            await _envioRepository.GuardarAsync();
        }
    }
}
