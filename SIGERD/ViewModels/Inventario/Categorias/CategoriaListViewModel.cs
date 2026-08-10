namespace SIGERD.ViewModels.Inventario.Categorias
{
    public class CategoriaListViewModel
    {
        public int IdCategoria { get; set; }

        public string NombreCategoria { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public int TotalArticulos { get; set; }
    }
}