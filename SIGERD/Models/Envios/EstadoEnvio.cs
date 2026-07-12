using SIGERD.Models.Auditoria;

namespace SIGERD.Models.Envios
{
    public class EstadoEnvio
    {
        public int idEstadoEnvio { get; set; }
        public string nombreEstadoEnvio { get; set; }
        public string? descripcion { get; set; } = null;
        public ICollection<Envio> Envios { get; set; } = new List<Envio>();
        public ICollection<HistorialMovimiento> HistorialMovimientos { get; set; } = new List<HistorialMovimiento>();
    }
}
