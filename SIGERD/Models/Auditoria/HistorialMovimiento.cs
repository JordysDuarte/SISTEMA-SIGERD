using SIGERD.Models.Envios;
using SIGERD.Models.Seguridad;

namespace SIGERD.Models.Auditoria
{
    public class HistorialMovimiento
    {
        public int idHistorialMovimiento { get; set; }
        public int idEnvioHistorialMovimiento { get; set; }
        public int idTipoMovimientoHistorialMovimiento { get; set; }
        public int idEstadoEnvioHistorialMovimiento { get; set; }
        public DateTime fechaMovimiento { get; set; }
        public int idUsuarioHistorialMovimiento { get; set; }
        public string? observaciones { get; set; } = null;

        public Envio Envio { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
        public EstadoEnvio EstadoEnvio { get; set; }
        public Usuario Usuario { get; set; }
    }
}
