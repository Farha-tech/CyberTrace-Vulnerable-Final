using Microsoft.AspNetCore.Mvc;

namespace CyberTrace.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // VULNERABLE:
            // No role or authorization check is performed.
            // Any logged-in user can access the Admin page.

            return View();
        }
    }
}
