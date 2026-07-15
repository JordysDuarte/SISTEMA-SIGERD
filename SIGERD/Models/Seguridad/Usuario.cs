using SIGERD.Models.Envios;
using SIGERD.Models.Recepciones;
using SIGERD.Models.Auditoria;
using SIGERD.Models.Ubicacion;

namespace SIGERD.Models.Seguridad
{
    public class Usuario
    {
        public int idUsuario { get; set; } 
        public string nombreCompleto { get; set; } = string.Empty;
        public string nombreUsuario { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
        public string claveHash { get; set; } = string.Empty; 
        public bool estado { get; set; }
        public bool debeCambiarClave { get; set; }
        public DateTime? fechaUltimoCambioClave { get; set; }
        public Guid versionSeguridad { get; set; }
        public int idRolUsuario { get; set; }
        public Rol Rol { get; set; } = null!;
        public int idDelegacionUsuario { get; set; }
        public Delegacion Delegacion { get; set; } = null!;
        public ICollection<Envio> Envios { get; set; } = new List<Envio>();
        public ICollection<Recepcion> Recepciones { get; set; } = new List<Recepcion>();
        public ICollection<HistorialMovimiento> HistorialMovimientos { get; set; } = new List<HistorialMovimiento>();
    }
}
