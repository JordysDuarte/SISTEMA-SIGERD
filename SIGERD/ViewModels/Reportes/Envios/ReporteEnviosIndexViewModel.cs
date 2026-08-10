namespace SIGERD.ViewModels.Reportes.Envios
{
    public class ReporteEnviosIndexViewModel
    {
        public ReporteEnviosFiltroViewModel Filtro { get; set; } = new();

        public List<ReporteEnviosResultadoViewModel> Resultados { get; set; } = new();

        public int TotalEnvios { get; set; }

        public int TotalPendientes { get; set; }

        public int TotalEnTransito { get; set; }

        public int TotalRecibidos { get; set; }
    }
}