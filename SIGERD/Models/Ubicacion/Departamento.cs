namespace SIGERD.Models.Ubicacion
{
    public class Departamento
    {
        public int idDepartamento { get; set; }
        public string nombreDepartamento { get; set; }
        public ICollection<Delegacion> Delegaciones { get; set; } = new List<Delegacion>();
    }
}
