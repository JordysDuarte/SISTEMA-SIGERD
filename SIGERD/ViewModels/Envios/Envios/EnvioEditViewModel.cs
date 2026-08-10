using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Envios.Envios
{
    public class EnvioEditViewModel
    {
        public int IdEnvio { get; set; }

        public string CodigoEnvio { get; set; } = string.Empty;

        public string EstadoEnvio { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; }

        [Display(Name = "Delegación origen")]
        public int IdDelegacionOrigen { get; set; }

        [Display(Name = "Delegación destino")]
        public int IdDelegacionDestino { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no deben superar los 500 caracteres.")]
        [Display(Name = "Observaciones generales")]
        public string? Observaciones { get; set; }

        public bool EsSuperAdministrador { get; set; }

        public string DelegacionOrigenUsuario { get; set; } = string.Empty;

        public List<DetalleEnvioEditViewModel> Detalles { get; set; } = new();

        public IEnumerable<SelectListItem> Delegaciones { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Articulos { get; set; } = new List<SelectListItem>();
    }

    public class DetalleEnvioEditViewModel
    {
        public int? IdDetalleEnvio { get; set; }

        [Display(Name = "Artículo")]
        public int? IdArticulo { get; set; }

        [Display(Name = "Cantidad")]
        public int? Cantidad { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no debe superar los 500 caracteres.")]
        [Display(Name = "Descripción")]
        public string? ObservacionesDetalles { get; set; }

        public IEnumerable<SelectListItem> Articulos { get; set; } = new List<SelectListItem>();
    }
}