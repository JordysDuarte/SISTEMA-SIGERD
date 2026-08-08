using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Recepciones
{
    public class RecepcionConfirmViewModel
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

        [StringLength(500, ErrorMessage = "La observación no debe superar los 500 caracteres.")]
        [Display(Name = "Observaciones de recepción")]
        public string? Observaciones { get; set; }

        public List<RecepcionDetalleViewModel> Detalles { get; set; } = new();
    }

    public class RecepcionDetalleViewModel
    {
        public string Articulo { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public string? ObservacionesDetalle { get; set; }
    }
}