using Microsoft.EntityFrameworkCore;
using Game.Models;

namespace Game.Helpers
{

    public class ApplicationDbContext : DbContext // dědí z Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<LeaderboardRecord> Records { get; set; } 
    }
}
