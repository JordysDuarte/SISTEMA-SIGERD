using SIGERD.Interfaces.IRespositories.Recepciones;
using SIGERD.Interfaces.IServices.Recepciones;
using SIGERD.Models.Envios;
using SIGERD.Models.Recepciones;

namespace SIGERD.Services.Recepciones
{
    public class RecepcionService : IRecepcionService
    {
        private readonly IRecepcionRepository _recepcionRepository;

        public RecepcionService(IRecepcionRepository recepcionRepository)
        {
            _recepcionRepository = recepcionRepository;
        }

        public async Task<IEnumerable<Envio>> ObtenerPendientesRecepcionAsync(
            int idDelegacionUsuario,
            bool esSuperAdministrador)
        {
            if (esSuperAdministrador)
            {
                return await _recepcionRepository.ObtenerEnviosEnTransitoAsync();
            }

            if (idDelegacionUsuario <= 0)
            {
                return new List<Envio>();
            }

            return await _recepcionRepository.ObtenerEnviosEnTransitoAsync(idDelegacionUsuario);
        }

        public async Task<Envio?> ObtenerEnvioParaConfirmarAsync(
            int idEnvio,
            int idDelegacionUsuario,
            bool esSuperAdministrador)
        {
            if (idEnvio <= 0)
            {
                return null;
            }

            var envio = await _recepcionRepository.ObtenerEnvioPorIdAsync(idEnvio);

            if (envio is null)
            {
                return null;
            }

            if (!EsEnvioEnTransito(envio))
            {
                return null;
            }

            if (envio.Recepcion != null)
            {
                return null;
            }

            if (esSuperAdministrador)
            {
                return envio;
            }

            if (idDelegacionUsuario <= 0)
            {
                return null;
            }

            if (envio.idDelegacionDestinoEnvio != idDelegacionUsuario)
            {
                return null;
            }

            return envio;
        }

        public async Task<int> ConfirmarRecepcionAsync(
            int idEnvio,
            int idUsuarioRecepcion,
            int idDelegacionUsuario,
            bool esSuperAdministrador,
            string? observaciones)
        {
            if (idEnvio <= 0)
            {
                throw new InvalidOperationException("El identificador del envío no es válido.");
            }

            if (idUsuarioRecepcion <= 0)
            {
                throw new InvalidOperationException("No fue posible identificar al usuario que recibe.");
            }

            var envio = await _recepcionRepository.ObtenerEnvioPorIdAsync(idEnvio);

            if (envio is null)
            {
                throw new InvalidOperationException("El envío solicitado no existe.");
            }

            if (!EsEnvioEnTransito(envio))
            {
                throw new InvalidOperationException("Solo se pueden recibir envíos en estado En tránsito.");
            }

            if (envio.Recepcion != null)
            {
                throw new InvalidOperationException("Este envío ya fue recibido.");
            }

            if (!esSuperAdministrador && envio.idDelegacionDestinoEnvio != idDelegacionUsuario)
            {
                throw new InvalidOperationException("No tienes permiso para recibir envíos de otra delegación.");
            }

            var recepcionExistente = await _recepcionRepository.ObtenerPorEnvioAsync(idEnvio);

            if (recepcionExistente != null)
            {
                throw new InvalidOperationException("Este envío ya tiene una recepción registrada.");
            }

            int? idEstadoRecibido = await _recepcionRepository.ObtenerIdEstadoPorNombreAsync("Recibido");

            if (!idEstadoRecibido.HasValue)
            {
                throw new InvalidOperationException("No existe el estado Recibido configurado en el sistema.");
            }

            var recepcion = new Recepcion
            {
                idEnvioRecepcion = idEnvio,
                idUsuarioRecepcion = idUsuarioRecepcion,
                fechaRecepcion = DateTime.Now,
                observaciones = observaciones?.Trim()
            };

            envio.idEstadoEnvioEnvio = idEstadoRecibido.Value;

            await _recepcionRepository.AgregarAsync(recepcion);
            _recepcionRepository.ActualizarEnvio(envio);
            await _recepcionRepository.GuardarAsync();

            return recepcion.idRecepcion;
        }

        public async Task<Recepcion?> ObtenerPorIdValidadoAsync(
            int idRecepcion,
            int idDelegacionUsuario,
            bool esSuperAdministrador)
        {
            if (idRecepcion <= 0)
            {
                return null;
            }

            var recepcion = await _recepcionRepository.ObtenerPorIdAsync(idRecepcion);

            if (recepcion is null || recepcion.Envio is null)
            {
                return null;
            }

            if (esSuperAdministrador)
            {
                return recepcion;
            }

            bool estaRelacionadaConDelegacion =
                recepcion.Envio.idDelegacionOrigenEnvio == idDelegacionUsuario ||
                recepcion.Envio.idDelegacionDestinoEnvio == idDelegacionUsuario;

            if (!estaRelacionadaConDelegacion)
            {
                return null;
            }

            return recepcion;
        }

        private bool EsEnvioEnTransito(Envio envio)
        {
            string estado = envio.EstadoEnvio?.nombreEstadoEnvio?.Trim() ?? string.Empty;

            return estado.Equals("En tránsito", StringComparison.OrdinalIgnoreCase) ||
                   estado.Equals("En transito", StringComparison.OrdinalIgnoreCase);
        }
    }
}