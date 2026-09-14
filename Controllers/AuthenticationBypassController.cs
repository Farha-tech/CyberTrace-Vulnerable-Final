using Microsoft.AspNetCore.Mvc;

namespace CyberTrace.Controllers
{
    public class AuthenticationBypassController : Controller
    {
        private const string CorrectUsername = "admin";
        private const string CorrectPassword = "admin123";

        // =========================
        // GET
        // =========================

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
            // Vulnerable authentication logic:
            // The application trusts a client-controlled
            // "role" value instead of verifying the real role.

            string role = Request.Form["role"];

            if (username == CorrectUsername &&
                password == CorrectPassword)
            {
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Role", "Admin");

                ViewBag.Success =
                    "Normal authentication successful. Admin access granted.";

                ViewBag.Role = "Admin";

                return View("VulnerableResult");
            }

            // Authentication bypass:
            // If the attacker submits role=Admin,
            // the application grants administrative access
            // without valid credentials.

            if (role == "Admin")
            {
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Role", "Admin");

                ViewBag.Success =
                    "Authentication bypass successful. Administrative access was granted without valid credentials.";

                ViewBag.Role = "Admin";

                return View("VulnerableResult");
            }

            ViewBag.Error =
                "Invalid username or password.";

            return View("VulnerableResult");
        }

        // =========================
        // Secure Version
        // =========================

        [HttpPost]
        public IActionResult Secure(string username, string password)
        {
            // The secure version completely ignores any
            // client-controlled role value.

            if (username != CorrectUsername ||
                password != CorrectPassword)
            {
                ViewBag.Error =
                    "Authentication failed. Valid credentials are required.";

                return View("SecureResult");
            }

            // Role is assigned by the server only after
            // successful authentication.

            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetString("Role", "Admin");

            ViewBag.Success =
                "Authentication successful. Admin access was granted after valid credential verification.";

            ViewBag.Role = "Admin";

            return View("SecureResult");
        }
    }
}