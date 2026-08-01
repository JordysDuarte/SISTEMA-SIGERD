using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SIGERD.ViewModels.Envios.Envios
{
    public class DetalleEnvioCreateViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un artículo.")]
        [Display(Name = "Artículo")]
        public int IdArticulo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        public IEnumerable<SelectListItem> Articulos { get; set; } = new List<SelectListItem>();
    }
}
