namespace SIGERD.Models.Inventario
{
    public class Categoria
    {
        public int idCategoria { get; set; }

        public string nombreCategoria { get; set; } = string.Empty;

        public bool estado { get; set; }

        public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
    }
}
