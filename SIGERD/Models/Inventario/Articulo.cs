using SIGERD.Models.Envios;

namespace SIGERD.Models.Inventario
{
    public class Articulo
    {
        public int idArticulo { get; set; }
        public string nombreArticulo { get; set; }
        public string? descripcion { get; set; }
        public int idCategoriaArticulo { get; set; }
        public Categoria Categoria { get; set; }
        public ICollection<DetalleEnvio> DetallesEnvio { get; set; } = new List<DetalleEnvio>();
    }
}
