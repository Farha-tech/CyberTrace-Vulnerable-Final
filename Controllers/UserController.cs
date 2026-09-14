using CyberTrace.Data;
using Microsoft.AspNetCore.Mvc;

namespace CyberTrace.Controllers
{
    public class UserController : Controller
    {
        private readonly CyberTraceDbContext _context;

        public UserController(CyberTraceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Profile(int id)
        {
            var currentUserId =
                HttpContext.Session.GetInt32("UserId");

            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user =
                _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Vulnerable:
            // No authorization check verifies that the requested
            // profile belongs to the currently authenticated user.

            return View(user);
        }
    }
}