using Microsoft.AspNetCore.Mvc;

namespace SIGERD.Controllers.Inventario
{
    public class ArticulosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
