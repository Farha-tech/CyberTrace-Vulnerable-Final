using Microsoft.AspNetCore.Mvc;

namespace CyberTrace.Controllers
{
    public class FuzzingController : Controller
    {
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
        public IActionResult Vulnerable(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                ViewBag.Error = "Please enter a value.";
                return View("Index");
            }

            try
            {
                // Vulnerable behavior:
                // The application accepts arbitrary input
                // without length or content validation.

                ViewBag.Input = input;
                ViewBag.Length = input.Length;

                if (input.Length > 100)
                {
                    ViewBag.Warning =
                        "The application accepted an unusually large input without validation.";
                }

                if (input.Contains("<script>",
                    StringComparison.OrdinalIgnoreCase))
                {
                    ViewBag.Warning =
                        "Potentially dangerous input was accepted without validation.";
                }

                return View("VulnerableResult");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                return View("VulnerableResult");
            }
        }

        // =========================
        // Secure Version
        // =========================

        [HttpPost]
        public IActionResult Secure(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                ViewBag.Error = "Input cannot be empty.";
                return View("Index");
            }

            // Maximum allowed input length
            const int maxLength = 50;

            if (input.Length > maxLength)
            {
                ViewBag.Error =
                    $"Input blocked. Maximum allowed length is {maxLength} characters.";

                return View("SecureResult");
            }

            // Reject control characters
            if (input.Any(char.IsControl))
            {
                ViewBag.Error =
                    "Input blocked because it contains invalid control characters.";

                return View("SecureResult");
            }

            // Reject obvious script payloads for this training lab
            if (input.Contains("<script>",
                    StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Error =
                    "Input blocked because potentially dangerous script content was detected.";

                return View("SecureResult");
            }

            ViewBag.Input = input;
            ViewBag.Length = input.Length;

            ViewBag.Success =
                "Input accepted after security validation.";

            return View("SecureResult");
        }
    }
}