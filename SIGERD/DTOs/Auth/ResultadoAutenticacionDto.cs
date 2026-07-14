namespace SIGERD.DTOs.Auth
{
    public class ResultadoAutenticacionDto
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public UsuarioSesionDto? Usuario { get; set; }


        public static ResultadoAutenticacionDto Correcto(UsuarioSesionDto usuario)
        {
            return new ResultadoAutenticacionDto
            {
                Exitoso = true,
                Usuario = usuario
            };
        }


        public static ResultadoAutenticacionDto Fallido(string mensaje)
        {
            return new ResultadoAutenticacionDto
            {
                Exitoso = false,
                Mensaje = mensaje
            };
        }
    }
}
