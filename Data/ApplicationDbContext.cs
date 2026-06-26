using Microsoft.EntityFrameworkCore;
using SmartNoticeBoardSystem.Models;

namespace SmartNoticeBoardSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Notice> Notices { get; set; }
    }
}