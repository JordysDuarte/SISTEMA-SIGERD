using Microsoft.AspNetCore.Mvc;

namespace SIGERD.Controllers.Envios
{
    public class EnviosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
