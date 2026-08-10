namespace SIGERD.ViewModels.Inventario.Articulos
{
    public class ArticuloDetailsViewModel
    {
        public int IdArticulo { get; set; }

        public string NombreArticulo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public string Categoria { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}