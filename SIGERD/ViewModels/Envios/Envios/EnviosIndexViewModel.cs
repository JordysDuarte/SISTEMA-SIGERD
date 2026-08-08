namespace SIGERD.ViewModels.Envios.Envios
{
    public class EnviosIndexViewModel
    {
        public string TipoVistaActual { get; set; } = string.Empty;

        public bool EsSuperAdministrador { get; set; }

        public IEnumerable<EnvioListViewModel> Envios { get; set; } = new List<EnvioListViewModel>();
    }
}
