using Microsoft.AspNetCore.Mvc;

namespace SIGERD.Controllers.Reportes
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
