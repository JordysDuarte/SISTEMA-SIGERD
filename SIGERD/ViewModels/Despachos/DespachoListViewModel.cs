namespace SIGERD.ViewModels.Despachos
{
    public class DespachoListViewModel
    {
        public int IdEnvio { get; set; }

        public string CodigoEnvio { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; }

        public string DelegacionOrigen { get; set; } = string.Empty;

        public string DelegacionDestino { get; set; } = string.Empty;

        public string UsuarioEnvio { get; set; } = string.Empty;

        public string EstadoEnvio { get; set; } = string.Empty;

        public int TotalArticulos { get; set; }

        public string? Observaciones { get; set; }
    }
}
