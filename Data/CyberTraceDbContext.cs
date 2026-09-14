using CyberTrace.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberTrace.Data
{
    public class CyberTraceDbContext : DbContext
    {
        public CyberTraceDbContext(DbContextOptions<CyberTraceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vulnerability> Vulnerabilities { get; set; }

        public DbSet<ScanResult> ScanResults { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<GuestMessage> GuestMessages { get; set; }

        public DbSet<SqlUser> SqlUsers { get; set; }
    }
}