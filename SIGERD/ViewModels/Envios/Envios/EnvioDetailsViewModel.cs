using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Envios.Envios
{
    public class EnvioDetailsViewModel
    {
        public int IdEnvio { get; set; }

        [Display(Name = "Código")]
        public string CodigoEnvio { get; set; } = string.Empty;

        [Display(Name = "Fecha")]
        public DateTime FechaEnvio { get; set; }

        [Display(Name = "Delegación Origen")]
        public string DelegacionOrigen { get; set; } = string.Empty;

        [Display(Name = "Delegación Destino")]
        public string DelegacionDestino { get; set; } = string.Empty;

        [Display(Name = "Usuario que registra")]
        public string UsuarioEnvio {  get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public string EstadoEnvio { get; set; } = string.Empty;

        [Display(Name = "Observaciones")]
        public string? Observaciones {  get; set; }

        public List<DetalleEnvioDetailsViewModel> Detalles { get; set; } = new();
    }

    public class DetalleEnvioDetailsViewModel
    {
        public int IdDetalleEnvio { get; set; }

        [Display(Name = "Artículo")]
        public string Articulo { get; set; } = string.Empty;

        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }
    }
}
