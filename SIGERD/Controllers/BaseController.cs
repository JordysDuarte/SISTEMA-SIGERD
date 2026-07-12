using Microsoft.AspNetCore.Mvc;

namespace SIGERD.Controllers
{
    public abstract class BaseController : Controller
    {
        protected void MostrarExito(string mensaje)
        {
            TempData["Success" ] = mensaje;
        }

        protected void MostrarError(string mensaje)
        {
            TempData["Error"] = mensaje;
        }

        protected void MostrarAdvertencia(string mensaje)
        {
            TempData["Warning"] = mensaje;
        }

        protected void MostrarInformation(string mensaje)
        {
            TempData["Info"] = mensaje;
        }
    }
}
