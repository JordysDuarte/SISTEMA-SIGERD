namespace SIGERD.ViewModels.Reportes.Envios
{
    public class ReporteEnviosResultadoViewModel
    {
        public int IdEnvio { get; set; }

        public string CodigoEnvio { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; }

        public string DelegacionOrigen { get; set; } = string.Empty;

        public string DelegacionDestino { get; set; } = string.Empty;

        public string UsuarioEnvio { get; set; } = string.Empty;

        public string EstadoEnvio { get; set; } = string.Empty;

        public DateTime? FechaDespacho { get; set; }

        public string? UsuarioDespacho { get; set; }

        public DateTime? FechaRecepcion { get; set; }

        public string? UsuarioRecepcion { get; set; }

        public int TotalArticulos { get; set; }
    }
}