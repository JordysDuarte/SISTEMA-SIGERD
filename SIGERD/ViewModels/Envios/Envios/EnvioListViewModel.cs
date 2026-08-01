using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Envios.Envios
{
    public class EnvioListViewModel
    {
        public int IdEnvio { get; set; }

        [Display(Name = "Código")]
        public string CodigoEnvio { get; set; } = string.Empty;

        [Display(Name = "Fecha")]
        public DateTime FechaEnvio { get; set; }

        [Display(Name = "Origen")]
        public string DelegacionOrigen { get; set; } = string.Empty;

        [Display(Name = "Destino")]
        public string DelegacionDestino {  get; set; } = string.Empty;

        [Display(Name = "Usuario")]
        public string UsuarioEnvio {  get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public string EstadoEnvio { get; set; } = string.Empty;

        [Display(Name = "Cantidad artículos")]
        public int TotalArticulos { get; set; }
    }
}
