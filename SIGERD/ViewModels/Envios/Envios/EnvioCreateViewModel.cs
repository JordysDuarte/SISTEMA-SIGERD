using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Envios.Envios
{
    public class EnvioCreateViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la delegación de origen.")]
        [Display(Name = "Delegación origen")]
        public int IdDelegacionOrigen { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la delegación destino.")]
        [Display(Name = "Delegación destino")]
        public int IdDelegacionDestino { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no debe superar los 500 carácteres.")]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        public List<DetalleEnvioCreateViewModel> Detalles { get; set; } = new();

        public IEnumerable<SelectListItem> Delegaciones { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Articulos { get; set; } = new List<SelectListItem>();
    }
}
