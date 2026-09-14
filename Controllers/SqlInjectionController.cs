using CyberTrace.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CyberTrace.Controllers
{
    public class SqlInjectionController : Controller
    {
        private readonly CyberTraceDbContext _context;

        public SqlInjectionController(CyberTraceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string username)
        {
            var users = new List<CyberTrace.Models.SqlUser>();

            if (!string.IsNullOrWhiteSpace(username))
            {
                var connection = _context.Database.GetDbConnection();

                connection.Open();

                using var command = connection.CreateCommand();

                command.CommandText =
                    $"SELECT Id, Username, Email, Role FROM SqlUsers WHERE Username = '{username}'";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new CyberTrace.Models.SqlUser
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Email = reader.GetString(2),
                        Role = reader.GetString(3)
                    });
                }

                connection.Close();
            }

            return View(users);
        }

        [HttpGet]
        public IActionResult Secure(string username)
        {
            var users = new List<CyberTrace.Models.SqlUser>();

            if (!string.IsNullOrWhiteSpace(username))
            {
                users = _context.SqlUsers
                    .Where(u => u.Username == username)
                    .ToList();
            }

            return View(users);
        }
    }
}