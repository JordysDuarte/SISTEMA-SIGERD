using Microsoft.AspNetCore.Mvc;

namespace SIGERD.Controllers.Recepciones
{
    public class RecepcionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
