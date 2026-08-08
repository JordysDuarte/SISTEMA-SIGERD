namespace SIGERD.Constants.Envios
{
    public static class TiposVistaEnvio
    {
        public const string Todos = "todos";
        public const string Enviados = "enviados";
        public const string Destinados = "destinados";

        public static string Normalizar(string? tipoVista, bool esSuperAdministrador)
        {
            if (string.IsNullOrWhiteSpace(tipoVista))
            {
                return esSuperAdministrador ? Todos : Enviados;
            }

            string tipo = tipoVista.Trim().ToLower();

            if (tipo == Todos && esSuperAdministrador)
            {
                return Todos;
            }

            if (tipo == Destinados)
            {
                return Destinados;
            }

            if (tipo == Enviados)
            {
                return Enviados;
            }

            return esSuperAdministrador ? Todos : Enviados;
        }
    }
}
