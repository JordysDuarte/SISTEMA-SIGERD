using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIGERD.Models.Seguridad;

namespace SIGERD.Data.Seed
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(
            ApplicationDbContext context,
            IPasswordHasher<Usuario> passwordHasher,
            IConfiguration configuration,
            ILogger<DataSeeder> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            bool existenUsuarios = await _context.Usuarios.AnyAsync();

            if (existenUsuarios)
            {
                return;
            }

            await CrearSuperAdministradorInicialAsync();
        }

        private async Task CrearSuperAdministradorInicialAsync()
        {
            string? claveInicial = _configuration["SeedAdmin:ClaveInicial"];

            if (string.IsNullOrWhiteSpace(claveInicial))
            {
                throw new InvalidOperationException(
                    "No se encontró la contraseña inicial del Super Administrador. Configure 'SeedAdmin:ClaveInicial' en User Secrets o variable de entorno."
                );
            }

            var rolSuperAdministrador = await _context.Roles
                .FirstOrDefaultAsync(r => r.nombreRol == "Super Administrador");

            if (rolSuperAdministrador is null)
            {
                throw new InvalidOperationException(
                    "No existe el rol 'Super Administrador'. Ejecute primero los scripts de datos base."
                );
            }

            var delegacionInicial = await _context.Delegaciones
                .OrderBy(d => d.idDelegacion)
                .FirstOrDefaultAsync();

            if (delegacionInicial is null)
            {
                throw new InvalidOperationException(
                    "No existe ninguna delegación registrada. Ejecute primero los scripts de datos base."
                );
            }

            string nombreUsuario = (_configuration["SeedAdmin:NombreUsuario"] ?? "admin")
                .Trim()
                .ToLower();

            bool existeNombreUsuario = await _context.Usuarios
                .AnyAsync(u => u.nombreUsuario == nombreUsuario);

            if (existeNombreUsuario)
            {
                return;
            }

            var usuario = new Usuario
            {
                nombreCompleto = _configuration["SeedAdmin:NombreCompleto"]
                    ?? "Super Administrador",

                nombreUsuario = nombreUsuario,

                correo = (_configuration["SeedAdmin:Correo"]
                    ?? "admin@sigerd.local").Trim().ToLower(),

                idRolUsuario = rolSuperAdministrador.idRol,

                idDelegacionUsuario = delegacionInicial.idDelegacion,

                estado = true,

                debeCambiarClave = true,

                fechaUltimoCambioClave = DateTime.UtcNow,

                versionSeguridad = Guid.NewGuid()
            };

            usuario.claveHash = _passwordHasher.HashPassword(
                usuario,
                claveInicial
            );

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Se creó el Super Administrador inicial con el usuario: {NombreUsuario}",
                usuario.nombreUsuario
            );
        }
    }
}