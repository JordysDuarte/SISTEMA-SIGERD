using SIGERD.Models.Inventario;

namespace SIGERD.Models.Envios
{
    public class DetalleEnvio
    {
        public int idDetalle{ get; set; }
        public int idEnvioDetalleEnvio { get; set; }
        public int idArticuloDetalleEnvio { get; set; }
        public int cantidad { get; set; }
        public string? observacionesDetalleEnvio { get; set; }
        public Envio? Envio { get; set; }
        public Articulo? Articulo { get; set; }
    }
}
