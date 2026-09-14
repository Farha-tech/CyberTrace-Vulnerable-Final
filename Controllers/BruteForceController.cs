using Microsoft.AspNetCore.Mvc;

namespace CyberTrace.Controllers
{
    public class BruteForceController : Controller
    {
        private const string CorrectUsername = "admin";
        private const string CorrectPassword = "admin123";

        private static int secureFailedAttempts = 0;
        private static DateTime secureBlockedUntil = DateTime.MinValue;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // =========================
        // Vulnerable Version
        // =========================

        [HttpPost]
        public IActionResult Vulnerable(string username, string password)
        {
            if (username == CorrectUsername &&
                password == CorrectPassword)
            {
                ViewBag.Success = "Login successful.";
            }
            else
            {
                ViewBag.Error = "Invalid username or password.";
            }

            ViewBag.Username = username;

            return View("VulnerableResult");
        }

        // =========================
        // Secure Version
        // =========================

        [HttpPost]
        public IActionResult Secure(string username, string password)
        {
            if (DateTime.Now < secureBlockedUntil)
            {
                var remainingSeconds =
                    (int)(secureBlockedUntil - DateTime.Now).TotalSeconds + 1;

                ViewBag.Error =
                    $"Too many failed attempts. Try again in {remainingSeconds} seconds.";

                return View("SecureResult");
            }

            if (username == CorrectUsername &&
                password == CorrectPassword)
            {
                secureFailedAttempts = 0;

                ViewBag.Success = "Login successful.";
            }
            else
            {
                secureFailedAttempts++;

                ViewBag.Error = "Invalid username or password.";

                if (secureFailedAttempts >= 3)
                {
                    secureBlockedUntil =
                        DateTime.Now.AddSeconds(30);

                    secureFailedAttempts = 0;

                    ViewBag.Error =
                        "Too many failed attempts. Login has been temporarily blocked for 30 seconds.";
                }
            }

            ViewBag.Username = username;

            return View("SecureResult");
        }
    }
}