using SIGERD.Models.Auditoria;
using SIGERD.Models.Recepciones;
using SIGERD.Models.Seguridad;
using SIGERD.Models.Ubicacion;

namespace SIGERD.Models.Envios
{
    public class Envio
    {
        public int idEnvio { get; set; }

        public string? codigoEnvio { get; set; }

        public DateTime fechaEnvio { get; set; }

        public int idDelegacionOrigenEnvio { get; set; }

        public int idDelegacionDestinoEnvio { get; set; }

        public int idUsuarioEnvio { get; set; }

        public int idEstadoEnvioEnvio { get; set; }

        public string? observaciones { get; set; } = null;

        public Delegacion? DelegacionOrigen { get; set; }

        public Delegacion? DelegacionDestino { get; set; }

        public Usuario? Usuario { get; set; }

        public EstadoEnvio? EstadoEnvio { get; set; }

        public ICollection<DetalleEnvio> DetallesEnvio { get; set; } = new List<DetalleEnvio>();

        public ICollection<HistorialMovimiento> HistorialMovimientos { get; set; } = new List<HistorialMovimiento>();

        public Recepcion? Recepcion { get; set; }
    }
}