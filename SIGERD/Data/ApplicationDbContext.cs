using Microsoft.EntityFrameworkCore;
using SIGERD.Models.Seguridad;
using SIGERD.Models.Ubicacion;
using SIGERD.Models.Inventario;
using SIGERD.Models.Envios;
using SIGERD.Models.Recepciones;
using SIGERD.Models.Auditoria;

namespace SIGERD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        // DbSets para el Esquema Seguridad
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // DbSets para el Esquema Ubicacion
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Delegacion> Delegaciones { get; set; }

        // DbSets para el Esquema Inventario
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Articulo> Articulos { get; set; }

        // DbSets para el Esquema Envios
        public DbSet<EstadoEnvio> EstadosEnvio { get; set; }
        public DbSet<Envio> Envios { get; set; }
        public DbSet<DetalleEnvio> DetalleEnvio { get; set; }

        // DbSets para el Esquema Recepciones
        public DbSet<Recepcion> Recepciones { get; set; }

        // DbSets para el Esquema Auditoria
        public DbSet<TipoMovimiento> TiposMovimiento { get; set; }

        public DbSet<HistorialMovimiento> HistorialMovimientos { get; set; }








        //METODO PARA CONFIGURAR LAS ENTIDADES Y SUS RELACIONES
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(
                ApplicationDbContext).Assembly);
        }
    }
}
