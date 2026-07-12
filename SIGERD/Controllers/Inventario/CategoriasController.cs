using Microsoft.AspNetCore.Mvc;

namespace SIGERD.Controllers.Inventario
{
    public class CategoriasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
