using CyberTrace.Data;
using CyberTrace.Models;
using Microsoft.AspNetCore.Mvc;

namespace CyberTrace.Controllers
{
    public class CsrfController : Controller
    {
        private readonly CyberTraceDbContext _context;

        public CsrfController(CyberTraceDbContext context)
        {
            _context = context;
        }

        // =========================
        // Vulnerable Version
        // =========================

        [HttpGet]
        public IActionResult Vulnerable()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);

            if (user == null)
            {
                return NotFound();
            }

            var model = new EmailChangeModel
            {
                CurrentEmail = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Vulnerable(EmailChangeModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.NewEmail;

            _context.SaveChanges();

            model.CurrentEmail = user.Email;

            ViewBag.Message =
                "Email changed successfully without CSRF protection.";

            return View(model);
        }

        // =========================
        // Secure Version
        // =========================

        [HttpGet]
        public IActionResult Secure()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);

            if (user == null)
            {
                return NotFound();
            }

            var model = new EmailChangeModel
            {
                CurrentEmail = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Secure(EmailChangeModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.NewEmail;

            _context.SaveChanges();

            model.CurrentEmail = user.Email;

            ViewBag.Message =
                "Email changed successfully with CSRF protection.";

            return View(model);
        }
    }
}