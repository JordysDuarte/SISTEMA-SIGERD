using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Reportes.Envios
{
    public class ReporteEnviosFiltroViewModel
    {
        [Display(Name = "Fecha inicio")]
        [DataType(DataType.Date)]
        public DateTime? FechaInicio { get; set; }

        [Display(Name = "Fecha fin")]
        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }

        [Display(Name = "Delegación origen")]
        public int? IdDelegacionOrigen { get; set; }

        [Display(Name = "Delegación destino")]
        public int? IdDelegacionDestino { get; set; }

        [Display(Name = "Estado")]
        public int? IdEstadoEnvio { get; set; }

        public IEnumerable<SelectListItem> Delegaciones { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> EstadosEnvio { get; set; } = new List<SelectListItem>();
    }
}