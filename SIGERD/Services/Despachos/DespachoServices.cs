using SIGERD.Interfaces.IRespositories.Despachos;
using SIGERD.Interfaces.IServices.Despachos;
using SIGERD.Models.Envios;

namespace SIGERD.Services.Despachos
{
    public class DespachoService : IDespachoService
    {
        private readonly IDespachoRepository _despachoRepository;

        public DespachoService(IDespachoRepository despachoRepository)
        {
            _despachoRepository = despachoRepository;
        }

        public async Task<IEnumerable<Envio>> ObtenerPendientesAsync(
            int idDelegacionUsuario,
            bool esSuperAdministrador)
        {
            if (esSuperAdministrador)
            {
                return await _despachoRepository.ObtenerPendientesAsync();
            }

            if (idDelegacionUsuario <= 0)
            {
                return new List<Envio>();
            }

            return await _despachoRepository.ObtenerPendientesAsync(idDelegacionUsuario);
        }

        public async Task DespacharAsync(
            int idEnvio,
            int idUsuarioDespacho,
            int idDelegacionUsuario,
            bool esSuperAdministrador)
        {
            if (idEnvio <= 0)
            {
                throw new InvalidOperationException("El identificador del envío no es válido.");
            }

            if (idUsuarioDespacho <= 0)
            {
                throw new InvalidOperationException("No fue posible identificar al usuario que despacha.");
            }

            var envio = await _despachoRepository.ObtenerPorIdAsync(idEnvio);

            if (envio is null)
            {
                throw new InvalidOperationException("El envío solicitado no existe.");
            }

            string estadoActual = envio.EstadoEnvio?.nombreEstadoEnvio?.Trim() ?? string.Empty;

            if (!estadoActual.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Solo se pueden despachar envíos en estado Pendiente.");
            }

            if (!esSuperAdministrador && envio.idDelegacionOrigenEnvio != idDelegacionUsuario)
            {
                throw new InvalidOperationException("No tienes permiso para despachar envíos de otra delegación.");
            }

            int? idEstadoEnTransito = await _despachoRepository.ObtenerIdEstadoPorNombreAsync("En tránsito");

            if (!idEstadoEnTransito.HasValue)
            {
                idEstadoEnTransito = await _despachoRepository.ObtenerIdEstadoPorNombreAsync("En transito");
            }

            if (!idEstadoEnTransito.HasValue)
            {
                throw new InvalidOperationException("No existe el estado En tránsito configurado en el sistema.");
            }

            envio.fechaDespacho = DateTime.Now;
            envio.idUsuarioDespacho = idUsuarioDespacho;
            envio.idEstadoEnvioEnvio = idEstadoEnTransito.Value;

            _despachoRepository.Actualizar(envio);
            await _despachoRepository.GuardarAsync();
        }
    }
}
