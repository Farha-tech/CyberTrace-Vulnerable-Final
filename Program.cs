using CyberTrace.Data;
using CyberTrace.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberTrace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            builder.Services.AddDbContext<CyberTraceDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // HTTP Client for SSRF Lab
            builder.Services.AddHttpClient();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            using (var scope = app.Services.CreateScope())
            {
                var context =
                    scope.ServiceProvider.GetRequiredService<CyberTraceDbContext>();

                // =========================
                // Default Users
                // =========================

                var adminUser =
                    context.Users.FirstOrDefault(u => u.Username == "admin");

                if (adminUser == null)
                {
                    context.Users.Add(
                        new User
                        {
                            Username = "admin",
                            Password = "admin123",
                            Role = "Admin",
                            Email = "admin@cybertrace.local",
                            FullName = "CyberTrace Administrator"
                        });
                }
                else
                {
                    adminUser.Password = "admin123";
                    adminUser.Role = "Admin";
                    adminUser.Email = "admin@cybertrace.local";
                    adminUser.FullName = "CyberTrace Administrator";
                }

                var normalUser =
                    context.Users.FirstOrDefault(u => u.Username == "user");

                if (normalUser == null)
                {
                    context.Users.Add(
                        new User
                        {
                            Username = "user",
                            Password = "user123",
                            Role = "User",
                            Email = "user@cybertrace.local",
                            FullName = "CyberTrace User"
                        });
                }
                else
                {
                    normalUser.Password = "user123";
                    normalUser.Role = "User";
                    normalUser.Email = "user@cybertrace.local";
                    normalUser.FullName = "CyberTrace User";
                }

                context.SaveChanges();

                // =========================
                // SQL Injection Lab Users
                // =========================

                if (!context.SqlUsers.Any())
                {
                    context.SqlUsers.AddRange(
                        new SqlUser
                        {
                            Username = "admin",
                            Email = "admin@sql-lab.local",
                            Role = "Admin"
                        },

                        new SqlUser
                        {
                            Username = "alice",
                            Email = "alice@sql-lab.local",
                            Role = "User"
                        },

                        new SqlUser
                        {
                            Username = "bob",
                            Email = "bob@sql-lab.local",
                            Role = "User"
                        }
                    );

                    context.SaveChanges();
                }
            }

            app.Run();
        }
    }
}