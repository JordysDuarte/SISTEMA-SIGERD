using SIGERD.Models.Envios;
using SIGERD.Models.Recepciones;
using SIGERD.Models.Auditoria;

namespace SIGERD.Models.Seguridad
{
    public class Usuario
    {
        public int idUsuario { get; set; } 
        public string nombreCompleto { get; set; }  
        public string correo { get; set; }
        public string clave { get; set; }  
        public bool estado { get; set; }
        public int idRolUsuario { get; set; }
        public Rol Rol { get; set; }
        public ICollection<Envio> Envios { get; set; } = new List<Envio>();
        public ICollection<Recepcion> Recepciones { get; set; } = new List<Recepcion>();
        public ICollection<HistorialMovimiento> HistorialMovimientos { get; set; } = new List<HistorialMovimiento>();
    }
}
