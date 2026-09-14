using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace CyberTrace.Controllers
{
    public class HttpSecurityController : Controller
    {
        private const string CorrectUsername = "admin";
        private const string CorrectPassword = "admin123";

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // =========================
        // Vulnerable HTTP Authentication
        // =========================

        [HttpGet]
        public IActionResult Vulnerable()
        {
            Response.Headers["X-Powered-By"] =
                "CyberTrace ASP.NET Core Lab";

            Response.Headers["X-Debug-Mode"] =
                "true";

            Response.Headers["X-Application-Environment"] =
                "Development";

            var authorizationHeader =
                Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authorizationHeader))
            {
                Response.StatusCode = 401;

                Response.Headers["WWW-Authenticate"] =
                    "Basic realm=\"CyberTrace Vulnerable Lab\"";

                ViewBag.Error =
                    "Authentication required. Send a Basic Authentication header.";

                return View("VulnerableResult");
            }

            try
            {
                var credentials =
                    AuthenticationHeaderValue.Parse(
                        authorizationHeader);

                if (!credentials.Scheme.Equals(
                        "Basic",
                        StringComparison.OrdinalIgnoreCase))
                {
                    Response.StatusCode = 401;

                    ViewBag.Error =
                        "Only Basic Authentication is supported.";

                    return View("VulnerableResult");
                }

                var decodedCredentials =
                    Encoding.UTF8.GetString(
                        Convert.FromBase64String(
                            credentials.Parameter ?? string.Empty));

                var parts =
                    decodedCredentials.Split(
                        ':',
                        2);

                if (parts.Length != 2)
                {
                    Response.StatusCode = 401;

                    ViewBag.Error =
                        "Invalid Basic Authentication format.";

                    return View("VulnerableResult");
                }

                var username = parts[0];
                var password = parts[1];

                if (username == CorrectUsername &&
                    password == CorrectPassword)
                {
                    ViewBag.Success =
                        "Authentication successful.";

                    ViewBag.Username = username;

                    return View("VulnerableResult");
                }

                ViewBag.Error =
                    "Invalid username or password.";

                Response.StatusCode = 401;

                return View("VulnerableResult");
            }
            catch
            {
                Response.StatusCode = 401;

                ViewBag.Error =
                    "Invalid Authorization header.";

                return View("VulnerableResult");
            }
        }


        // =========================
        // Secure HTTP Authentication
        // =========================

        [HttpGet]
        public IActionResult Secure()
        {
            AddSecurityHeaders();

            if (!Request.IsHttps)
            {
                Response.StatusCode = 403;

                ViewBag.Error =
                    "Secure authentication requires HTTPS.";

                return View("SecureResult");
            }

            var authorizationHeader =
                Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authorizationHeader))
            {
                Response.StatusCode = 401;

                Response.Headers["WWW-Authenticate"] =
                    "Basic realm=\"CyberTrace Secure Lab\"";

                ViewBag.Error =
                    "Authentication required.";

                return View("SecureResult");
            }

            try
            {
                var credentials =
                    AuthenticationHeaderValue.Parse(
                        authorizationHeader);

                if (!credentials.Scheme.Equals(
                        "Basic",
                        StringComparison.OrdinalIgnoreCase))
                {
                    Response.StatusCode = 401;

                    ViewBag.Error =
                        "Only Basic Authentication is supported.";

                    return View("SecureResult");
                }

                var decodedCredentials =
                    Encoding.UTF8.GetString(
                        Convert.FromBase64String(
                            credentials.Parameter ?? string.Empty));

                var parts =
                    decodedCredentials.Split(
                        ':',
                        2);

                if (parts.Length != 2)
                {
                    Response.StatusCode = 401;

                    ViewBag.Error =
                        "Invalid authentication format.";

                    return View("SecureResult");
                }

                var username = parts[0];
                var password = parts[1];

                if (username != CorrectUsername ||
                    password != CorrectPassword)
                {
                    Response.StatusCode = 401;

                    ViewBag.Error =
                        "Authentication failed.";

                    return View("SecureResult");
                }

                ViewBag.Success =
                    "Secure authentication successful.";

                ViewBag.Username = username;

                return View("SecureResult");
            }
            catch
            {
                Response.StatusCode = 401;

                ViewBag.Error =
                    "Invalid Authorization header.";

                return View("SecureResult");
            }
        }


        // =========================
        // Security Headers
        // =========================

        private void AddSecurityHeaders()
        {
            Response.Headers.Remove("X-Powered-By");
            Response.Headers.Remove("X-Debug-Mode");
            Response.Headers.Remove("X-Application-Environment");

            Response.Headers["X-Content-Type-Options"] =
                "nosniff";

            Response.Headers["X-Frame-Options"] =
                "DENY";

            Response.Headers["Referrer-Policy"] =
                "strict-origin-when-cross-origin";

            Response.Headers["Permissions-Policy"] =
                "geolocation=(), microphone=(), camera=()";

            Response.Headers["Content-Security-Policy"] =
                "default-src 'self'; object-src 'none'; frame-ancestors 'none'; base-uri 'self'";
        }
    }
}