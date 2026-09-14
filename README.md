# CyberTrace - Vulnerable Version

CyberTrace is a web security educational project designed to demonstrate common web application vulnerabilities and their exploitation in a controlled environment.

## Project Overview

This version of CyberTrace intentionally contains vulnerable implementations to demonstrate how common web security vulnerabilities can occur in web applications.

The project is intended for educational and security testing purposes only.

## Vulnerabilities Demonstrated

The project includes the following 15 security cases:

1. SQL Injection
2. Reflected XSS
3. Stored XSS
4. DOM XSS
5. Cross-Site Request Forgery (CSRF)
6. Authentication Bypass
7. Brute Force
8. Command Injection
9. Server-Side Request Forgery (SSRF)
10. Information Disclosure
11. HTTP Security Misconfiguration
12. Dictionary Attack
13. Fuzzing / Improper Input Validation
14. Insecure Direct Object Reference (IDOR)
15. Broken Admin Access Control

## Technologies Used

- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- HTML
- CSS
- JavaScript

## Project Structure

The project follows a standard ASP.NET Core MVC structure:

- `Controllers/` - Application controllers
- `Models/` - Data models
- `Data/` - Database context and data configuration
- `Views/` - MVC views
- `wwwroot/` - Static files
- `Properties/` - Project configuration
- `Program.cs` - Application entry point
- `appsettings.json` - Application configuration

## Purpose

The vulnerable version is used to demonstrate how insecure implementations can be exploited and why proper security controls are necessary.

A separate secure version of CyberTrace implements security protections for the demonstrated vulnerabilities.

## Disclaimer

This project is created for educational and authorized security testing purposes only.

Do not use the demonstrated techniques against systems or applications without proper authorization.
