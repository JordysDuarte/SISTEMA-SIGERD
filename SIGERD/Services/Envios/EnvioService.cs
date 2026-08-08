using Microsoft.IdentityModel.Tokens;
using SIGERD.Constants.Envios;
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


        public async Task<IEnumerable<Envio>> ObtenerPorVistaAsync(
            string? tipoVista,
            int idDelegacionUsuario,
            bool esSuperAdministrador)
        {
            string tipoVistaNormalizada = TiposVistaEnvio.Normalizar(
                tipoVista,
                esSuperAdministrador
            );

            if (tipoVistaNormalizada == TiposVistaEnvio.Todos && esSuperAdministrador)
            {
                return await _envioRepository.ObtenerTodosAsync();
            }

            if (idDelegacionUsuario <= 0)
            {
                return new List<Envio>();
            }

            if (tipoVistaNormalizada == TiposVistaEnvio.Destinados)
            {
                return await _envioRepository.ObtenerPorDelegacionDestinoAsync(idDelegacionUsuario);
            }

            return await _envioRepository.ObtenerPorDelegacionOrigenAsync(idDelegacionUsuario);
        }

        public async Task<Envio?> ObtenerPorIdValidadoAsync(
            int idEnvio,
            int idDelegacionUsuario,
            bool esSuperAdministrador)
        {
            if (idEnvio <= 0)
            {
                return null;
            }

            var envio = await _envioRepository.ObtenerPorIdAsync(idEnvio);

            if (envio is null)
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

            bool envioRelacionadoConDelegacion =
                envio.idDelegacionOrigenEnvio == idDelegacionUsuario ||
                envio.idDelegacionDestinoEnvio == idDelegacionUsuario;

            if (!envioRelacionadoConDelegacion)
            {
                return null;
            }

            return envio;
        }

        public async Task<int> CrearAsync(
    Envio envio,
    int idDelegacionUsuario,
    bool esSuperAdministrador)
        {
            if (envio is null)
            {
                throw new InvalidOperationException("La información del envío no es válida.");
            }

            if (!esSuperAdministrador)
            {
                if (idDelegacionUsuario <= 0)
                {
                    throw new InvalidOperationException("No fue posible identificar la delegación del usuario.");
                }

                envio.idDelegacionOrigenEnvio = idDelegacionUsuario;
            }

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
                throw new InvalidOperationException("No fue posible identificar al usuario que registra el envío.");
            }

            bool existeDelegacionOrigen = await _envioRepository.ExisteDelegacionAsync(
                envio.idDelegacionOrigenEnvio
            );

            if (!existeDelegacionOrigen)
            {
                throw new InvalidOperationException("La delegación origen no existe.");
            }

            bool existeDelegacionDestino = await _envioRepository.ExisteDelegacionAsync(
                envio.idDelegacionDestinoEnvio
            );

            if (!existeDelegacionDestino)
            {
                throw new InvalidOperationException("La delegación destino no existe.");
            }

            if (envio.DetallesEnvio == null || !envio.DetallesEnvio.Any())
            {
                throw new InvalidOperationException("Debe agregar al menos un artículo al envío.");
            }

            var articulosRegistrados = new HashSet<int>();

            foreach (var detalle in envio.DetallesEnvio)
            {
                if (detalle.idArticuloDetalleEnvio <= 0)
                {
                    throw new InvalidOperationException("Debe seleccionar un artículo válido.");
                }

                if (detalle.cantidad <= 0)
                {
                    throw new InvalidOperationException("La cantidad debe ser mayor que cero.");
                }

                if (!articulosRegistrados.Add(detalle.idArticuloDetalleEnvio))
                {
                    throw new InvalidOperationException("No se permite repetir el mismo artículo en el mismo envío.");
                }

                bool existeArticulo = await _envioRepository.ExisteArticuloAsync(
                    detalle.idArticuloDetalleEnvio
                );

                if (!existeArticulo)
                {
                    throw new InvalidOperationException("Uno de los artículos seleccionados no existe.");
                }

                detalle.observacionesDetalleEnvio = detalle.observacionesDetalleEnvio?.Trim();
            }

            int? idEstadoInicial = await _envioRepository.ObtenerIdEstadoInicialAsync();

            if (!idEstadoInicial.HasValue)
            {
                throw new InvalidOperationException("No existe un estado inicial configurado para el envío.");
            }

            DateTime fechaActual = DateTime.Now;

            int consecutivo = await _envioRepository.ObtenerConsecutivoDiarioAsync(fechaActual);

            envio.fechaEnvio = fechaActual;
            envio.codigoEnvio = $"ENV-{fechaActual:yyyyMMdd}-{consecutivo:0000}";
            envio.idEstadoEnvioEnvio = idEstadoInicial.Value;
            envio.observaciones = envio.observaciones?.Trim();

            await _envioRepository.AgregarAsync(envio);
            await _envioRepository.GuardarAsync();

            return envio.idEnvio;
        }

        //private async Task<string> GenerarCodigoAsync(DateTime fecha)
        //{
        //    int consecutivo = await _envioRepository.ObtenerConsecutivoDiarioAsync(fecha);
        //    return $"ENV-{fecha:yyyyMMdd}-{consecutivo:0000}";
        //}

    }
}
