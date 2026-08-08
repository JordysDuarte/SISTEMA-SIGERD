namespace SIGERD.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public bool EsSuperAdministrador { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string NombreDelegacion { get; set; } = string.Empty;

        public string TituloContexto { get; set; } = string.Empty;

        public int TotalUsuarios { get; set; }

        public int TotalArticulos { get; set; }

        public int TotalEnvios { get; set; }

        public int TotalRecepciones { get; set; }

        public int EnviosEnviados { get; set; }

        public int EnviosDestinados { get; set; }

        public int PendientesDespacho { get; set; }

        public int PendientesRecepcion { get; set; }

        public int EnviosEnTransito { get; set; }

        public int EnviosRecibidos { get; set; }

        public List<DashboardEnvioViewModel> UltimosEnvios { get; set; } = new();
    }

    public class DashboardEnvioViewModel
    {
        public int IdEnvio { get; set; }

        public string CodigoEnvio { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; }

        public string DelegacionOrigen { get; set; } = string.Empty;

        public string DelegacionDestino { get; set; } = string.Empty;

        public string EstadoEnvio { get; set; } = string.Empty;
    }
}