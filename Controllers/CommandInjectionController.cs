using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CyberTrace.Controllers
{
    public class CommandInjectionController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // =========================
        // Vulnerable Version
        // =========================

        [HttpPost]
        public IActionResult Vulnerable(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                ViewBag.Error = "Please enter a command.";
                return View("Index");
            }

            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new Process();

                process.StartInfo = processInfo;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                ViewBag.Command = command;
                ViewBag.Output = output;
                ViewBag.ErrorOutput = error;

                return View("Result");
            }
            catch (Exception ex)
            {
                ViewBag.Command = command;
                ViewBag.Error = ex.Message;

                return View("Result");
            }
        }

        // =========================
        // Secure Version
        // =========================

        [HttpPost]
        public IActionResult Secure(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                ViewBag.Error = "Please enter a command.";
                return View("Index");
            }

            var allowedCommands = new Dictionary<string, string>
            {
                { "whoami", "whoami" },
                { "hostname", "hostname" },
                { "date", "date /t" }
            };

            if (!allowedCommands.TryGetValue(
                    command.Trim().ToLower(),
                    out string? safeCommand))
            {
                ViewBag.Error =
                    "Command blocked. Only approved commands are allowed.";

                return View("Index");
            }

            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + safeCommand,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new Process();

                process.StartInfo = processInfo;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                ViewBag.Command = command;
                ViewBag.Output = output;
                ViewBag.ErrorOutput = error;

                return View("SecureResult");
            }
            catch (Exception ex)
            {
                ViewBag.Command = command;
                ViewBag.Error = ex.Message;

                return View("SecureResult");
            }
        }
    }
}