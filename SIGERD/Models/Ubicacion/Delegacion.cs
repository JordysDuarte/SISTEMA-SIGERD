namespace SIGERD.Models.Ubicacion
{
    public class Delegacion
    {
        public int idDelegacion { get; set; }
        public string nombreDelegacion { get; set; }
        public string? direccion { get; set; }
        public string? telefono { get; set; }
        public int idDepartamentoDelegacion { get; set; }
        public Departamento Departamento { get; set; }
        public ICollection<Envios.Envio> Envios { get; set; } = new List<Envios.Envio>();
    }
}
