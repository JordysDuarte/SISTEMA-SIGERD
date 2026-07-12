using SIGERD.Models.Envios;
using SIGERD.Models.Seguridad;

namespace SIGERD.Models.Recepciones
{
    public class Recepcion
    {
        public int idRecepcion { get; set; }
        public int idEnvioRecepcion { get; set; }
        public DateTime fechaRecepcion { get; set; }
        public int idUsuarioRecepcion { get; set; }
        public string? observaciones { get; set; }

        public Envio Envio { get; set; }
        public Usuario Usuario { get; set; }
    }
}
