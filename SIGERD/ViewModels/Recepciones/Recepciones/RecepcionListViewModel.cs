namespace SIGERD.ViewModels.Recepciones
{
    public class RecepcionEnvioListViewModel
    {
        public int IdEnvio { get; set; }

        public string CodigoEnvio { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; }

        public DateTime? FechaDespacho { get; set; }

        public string DelegacionOrigen { get; set; } = string.Empty;

        public string DelegacionDestino { get; set; } = string.Empty;

        public string UsuarioEnvio { get; set; } = string.Empty;

        public string? UsuarioDespacho { get; set; }

        public string EstadoEnvio { get; set; } = string.Empty;

        public int TotalArticulos { get; set; }
    }
}