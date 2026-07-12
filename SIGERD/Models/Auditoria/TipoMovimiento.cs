namespace SIGERD.Models.Auditoria
{
    public class TipoMovimiento
    {
        public int idTipoMovimiento { get; set; }
        public string nombreMovimiento { get; set; }
        public string? descripcion { get; set; } = null;
        public ICollection<HistorialMovimiento> HistorialMovimientos { get; set; } = new List<HistorialMovimiento>();
    }
}
