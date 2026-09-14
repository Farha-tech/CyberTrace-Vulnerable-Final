using Microsoft.AspNetCore.Mvc;
using CyberTrace.Models;

namespace CyberTrace.Controllers
{
    public class ScanController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string targetUrl)
        {
            var result = new ScanResult
            {
                Id = 1,
                TargetUrl = targetUrl,
                ScanDate = DateTime.Now,
                IsSafe = false,

                Vulnerabilities = new List<Vulnerability>
                {
                    new Vulnerability
                    {
                        Id = 1,
                        Name = "SQL Injection",
                        Category = "Injection",
                        Severity = "High",
                        Description =
                            "SQL Injection occurs when untrusted user input is directly included in a database query.",
                        AttackScenario =
                            "An attacker may manipulate input parameters to modify the intended SQL query and access unauthorized data.",
                        Recommendation =
                            "Use parameterized queries, prepared statements, and proper input validation.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 2,
                        Name = "Reflected XSS",
                        Category = "Injection",
                        Severity = "Medium",
                        Description =
                            "Reflected XSS occurs when user input is immediately returned in a web page without proper output encoding.",
                        AttackScenario =
                            "An attacker may send a malicious URL containing JavaScript that executes in the victim's browser.",
                        Recommendation =
                            "Properly encode user-controlled output and validate input.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 3,
                        Name = "Stored XSS",
                        Category = "Injection",
                        Severity = "High",
                        Description =
                            "Stored XSS occurs when malicious user input is stored by the application and later rendered without proper encoding.",
                        AttackScenario =
                            "An attacker may store malicious JavaScript that executes whenever another user views the affected content.",
                        Recommendation =
                            "Encode stored user input before rendering it and apply proper input validation.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 4,
                        Name = "DOM XSS",
                        Category = "Injection",
                        Severity = "Medium",
                        Description =
                            "DOM XSS occurs when client-side JavaScript inserts untrusted data into the page using unsafe DOM operations.",
                        AttackScenario =
                            "An attacker may provide a malicious URL parameter that is inserted into the page and executed as JavaScript.",
                        Recommendation =
                            "Use safe DOM APIs such as textContent instead of innerHTML for untrusted data.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 5,
                        Name = "CSRF",
                        Category = "Broken Access Control",
                        Severity = "High",
                        Description =
                            "CSRF allows an attacker to force an authenticated user to perform an unwanted action.",
                        AttackScenario =
                            "An attacker may create a malicious page that sends a forged request to a vulnerable application.",
                        Recommendation =
                            "Use anti-forgery tokens and validate requests on the server side.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 6,
                        Name = "Authentication Bypass",
                        Category = "Broken Authentication",
                        Severity = "High",
                        Description =
                            "Authentication bypass occurs when an attacker can gain authenticated or administrative access without valid credentials.",
                        AttackScenario =
                            "An attacker may manipulate authentication parameters or role values to obtain unauthorized access.",
                        Recommendation =
                            "Verify credentials and authorization on the server side and never trust client-controlled role values.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 7,
                        Name = "Brute Force",
                        Category = "Broken Authentication",
                        Severity = "High",
                        Description =
                            "Brute force attacks attempt multiple username and password combinations until valid credentials are discovered.",
                        AttackScenario =
                            "An attacker may repeatedly submit incorrect passwords against a login endpoint.",
                        Recommendation =
                            "Implement rate limiting, account lockout, and monitoring for repeated failed login attempts.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 8,
                        Name = "Command Injection",
                        Category = "Injection",
                        Severity = "Critical",
                        Description =
                            "Command Injection occurs when user-controlled input is passed directly to an operating system command.",
                        AttackScenario =
                            "An attacker may inject additional operating system commands through a vulnerable input field.",
                        Recommendation =
                            "Avoid executing user-controlled commands and use strict allowlists for required operations.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 9,
                        Name = "SSRF",
                        Category = "Server-Side Request Forgery",
                        Severity = "High",
                        Description =
                            "SSRF occurs when a server makes HTTP requests to attacker-controlled or internal resources.",
                        AttackScenario =
                            "An attacker may provide an internal URL and force the server to access protected resources.",
                        Recommendation =
                            "Validate URLs, allow only approved destinations, and block localhost and private network addresses.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 10,
                        Name = "Information Disclosure",
                        Category = "Information Disclosure",
                        Severity = "Medium",
                        Description =
                            "Information Disclosure occurs when sensitive application or system information is exposed to users.",
                        AttackScenario =
                            "An attacker may access an endpoint that reveals internal paths, environment information, or system details.",
                        Recommendation =
                            "Do not expose sensitive system information and return only the data required by the user.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 11,
                        Name = "HTTP Security Misconfiguration",
                        Category = "Security Misconfiguration",
                        Severity = "Medium",
                        Description =
                            "HTTP Security Misconfiguration occurs when security response headers are missing or sensitive debugging headers are exposed.",
                        AttackScenario =
                            "An attacker may use exposed environment or debugging information or exploit missing browser security protections.",
                        Recommendation =
                            "Configure security headers such as CSP, X-Frame-Options, X-Content-Type-Options, and Referrer-Policy.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 12,
                        Name = "Dictionary Attack",
                        Category = "Broken Authentication",
                        Severity = "High",
                        Description =
                            "Dictionary attacks attempt commonly used passwords against an authentication endpoint.",
                        AttackScenario =
                            "An attacker may repeatedly submit password guesses from a prepared dictionary.",
                        Recommendation =
                            "Limit authentication attempts and temporarily block repeated failed requests.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 13,
                        Name = "Fuzzing / Improper Input Validation",
                        Category = "Input Validation",
                        Severity = "Medium",
                        Description =
                            "Improper input validation occurs when applications accept unexpected, oversized, or potentially dangerous input.",
                        AttackScenario =
                            "An attacker may submit unusually large or malformed input to test application boundaries or trigger unexpected behavior.",
                        Recommendation =
                            "Validate input length, format, content, and control characters before processing.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 14,
                        Name = "IDOR",
                        Category = "Broken Access Control",
                        Severity = "High",
                        Description =
                            "IDOR occurs when an application allows users to access objects belonging to other users without proper authorization checks.",
                        AttackScenario =
                            "An attacker may change an object identifier in the URL and access another user's profile.",
                        Recommendation =
                            "Verify that the requested object belongs to the currently authenticated user before returning it.",
                        IsFixed = false
                    },

                    new Vulnerability
                    {
                        Id = 15,
                        Name = "Broken Admin Access Control",
                        Category = "Broken Access Control",
                        Severity = "High",
                        Description =
                            "Broken access control occurs when users can access administrative functionality without the required privileges.",
                        AttackScenario =
                            "A normal user may attempt to directly access an administrative URL.",
                        Recommendation =
                            "Verify the user's role and authorization before allowing access to administrative resources.",
                        IsFixed = false
                    }
                }
            };

            return View("Result", result);
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}