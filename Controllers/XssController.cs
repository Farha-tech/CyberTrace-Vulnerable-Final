using CyberTrace.Data;
using Microsoft.AspNetCore.Mvc;

namespace CyberTrace.Controllers
{
    public class XssController : Controller
    {
        private readonly CyberTraceDbContext _context;

        public XssController(CyberTrace.Data.CyberTraceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string input)
        {
            ViewBag.Input = input;

            return View();
        }

        [HttpGet]
        public IActionResult Secure(string input)
        {
            ViewBag.Input = input;

            return View();
        }

        // =========================
        // Vulnerable Stored XSS
        // =========================

        [HttpGet]
        public IActionResult Stored()
        {
            var messages = _context.GuestMessages
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            return View(messages);
        }

        [HttpPost]
        public IActionResult Stored(string username, string message)
        {
            var guestMessage = new CyberTrace.Models.GuestMessage
            {
                Username = username,
                Message = message,
                CreatedAt = DateTime.Now
            };

            _context.GuestMessages.Add(guestMessage);
            _context.SaveChanges();

            return RedirectToAction("Stored");
        }

        // =========================
        // Secure Stored XSS
        // =========================

        [HttpGet]
        public IActionResult StoredSecure()
        {
            var messages = _context.GuestMessages
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            return View(messages);
        }

        [HttpPost]
        public IActionResult StoredSecure(string username, string message)
        {
            var guestMessage = new CyberTrace.Models.GuestMessage
            {
                Username = username,
                Message = message,
                CreatedAt = DateTime.Now
            };

            _context.GuestMessages.Add(guestMessage);
            _context.SaveChanges();

            return RedirectToAction("StoredSecure");
        }

        // =========================
        // DOM XSS
        // =========================

        [HttpGet]
        public IActionResult Dom()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DomSecure()
        {
            return View();
        }
    }
}