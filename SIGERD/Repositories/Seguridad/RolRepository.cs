using SIGERD.Data;
using SIGERD.Interfaces.IRespositories.Seguridad;
using SIGERD.Models.Seguridad;
using SIGERD.Repositories.Base;

namespace SIGERD.Repositories.Seguridad
{
    public class RolRepository : GenericRepository<Rol>, IRolRepository
    {
        public RolRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}
