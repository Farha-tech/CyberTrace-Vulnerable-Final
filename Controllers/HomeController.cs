using Microsoft.AspNetCore.Mvc;

namespace CyberTrace.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}