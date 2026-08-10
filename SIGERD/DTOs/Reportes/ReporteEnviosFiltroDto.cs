namespace SIGERD.DTOs.Reportes
{
    public class ReporteEnviosFiltroDto
    {
        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public int? IdDelegacionOrigen { get; set; }

        public int? IdDelegacionDestino { get; set; }

        public int? IdEstadoEnvio { get; set; }

        public int IdDelegacionUsuario { get; set; }

        public bool EsSuperAdministrador { get; set; }
    }
}