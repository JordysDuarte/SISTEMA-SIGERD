using Microsoft.AspNetCore.Mvc;

namespace SIGERD.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
