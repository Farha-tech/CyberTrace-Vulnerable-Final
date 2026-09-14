using Microsoft.AspNetCore.Mvc;

namespace CyberTrace.Controllers
{
    public class InformationDisclosureController : Controller
    {
        // =========================
        // Information Disclosure Lab
        // =========================

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // =========================
        // Vulnerable Version
        // =========================

        [HttpGet]
        public IActionResult Vulnerable()
        {
            var sensitiveInformation = new
            {
                Application = "CyberTrace",
                Environment = "Development",
                DatabaseServer = "localhost",
                DatabaseName = "CyberTraceDb",
                DatabaseProvider = "Microsoft SQL Server",
                InternalPath = AppContext.BaseDirectory,
                Framework = Environment.Version.ToString(),
                OperatingSystem = Environment.OSVersion.ToString(),
                MachineName = Environment.MachineName,
                UserName = Environment.UserName
            };

            return Json(sensitiveInformation);
        }

        // =========================
        // Secure Version
        // =========================

        [HttpGet]
        public IActionResult Secure()
        {
            var safeInformation = new
            {
                Application = "CyberTrace",
                Status = "Operational",
                Message = "Sensitive system information is protected."
            };

            return Json(safeInformation);
        }
    }
}