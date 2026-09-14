using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CyberTrace.Controllers
{
    public class SsrfController : Controller
    {
        private readonly HttpClient _httpClient;

        public SsrfController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        // =========================
        // SSRF Lab Home
        // =========================

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // =========================
        // Internal Resource
        // =========================

        [HttpGet]
        public IActionResult InternalResource()
        {
            return Content(
                "CYBERTRACE INTERNAL RESOURCE\n" +
                "Secret Data: INTERNAL-SECRET-2026\n" +
                "This resource should not be accessible through SSRF."
            );
        }

        // =========================
        // Vulnerable SSRF
        // =========================

        [HttpPost]
        public async Task<IActionResult> Vulnerable(string targetUrl)
        {
            if (string.IsNullOrWhiteSpace(targetUrl))
            {
                ViewBag.Error = "Please enter a URL.";
                return View("Index");
            }

            try
            {
                var response = await _httpClient.GetAsync(targetUrl);

                var content = await response.Content.ReadAsStringAsync();

                ViewBag.TargetUrl = targetUrl;
                ViewBag.StatusCode = (int)response.StatusCode;
                ViewBag.Response = content;

                return View("Result");
            }
            catch (Exception ex)
            {
                ViewBag.TargetUrl = targetUrl;
                ViewBag.Error = ex.Message;

                return View("Result");
            }
        }

        // =========================
        // Secure SSRF
        // =========================

        [HttpPost]
        public async Task<IActionResult> Secure(string targetUrl)
        {
            if (string.IsNullOrWhiteSpace(targetUrl))
            {
                ViewBag.Error = "Please enter a URL.";
                return View("Index");
            }

            if (!Uri.TryCreate(targetUrl, UriKind.Absolute, out Uri? uri))
            {
                ViewBag.Error = "Invalid URL.";
                return View("Index");
            }

            if (uri.Scheme != Uri.UriSchemeHttps)
            {
                ViewBag.Error = "Only HTTPS URLs are allowed.";
                return View("Index");
            }

            if (IsPrivateOrLocalAddress(uri.Host))
            {
                ViewBag.Error = "Access to private or local addresses is blocked.";
                return View("Index");
            }

            try
            {
                var response = await _httpClient.GetAsync(uri);

                var content = await response.Content.ReadAsStringAsync();

                ViewBag.TargetUrl = targetUrl;
                ViewBag.StatusCode = (int)response.StatusCode;
                ViewBag.Response = content;

                return View("SecureResult");
            }
            catch (Exception ex)
            {
                ViewBag.TargetUrl = targetUrl;
                ViewBag.Error = ex.Message;

                return View("SecureResult");
            }
        }

        // =========================
        // SSRF Protection
        // =========================

        private bool IsPrivateOrLocalAddress(string host)
        {
            if (host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (host.Equals("::1", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (IPAddress.TryParse(host, out IPAddress? ipAddress))
            {
                if (IPAddress.IsLoopback(ipAddress))
                {
                    return true;
                }

                if (ipAddress.AddressFamily ==
                    System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    byte[] bytes = ipAddress.GetAddressBytes();

                    // 10.0.0.0/8
                    if (bytes[0] == 10)
                    {
                        return true;
                    }

                    // 172.16.0.0/12
                    if (bytes[0] == 172 &&
                        bytes[1] >= 16 &&
                        bytes[1] <= 31)
                    {
                        return true;
                    }

                    // 192.168.0.0/16
                    if (bytes[0] == 192 &&
                        bytes[1] == 168)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}