using Microsoft.EntityFrameworkCore;
using Game.Models;

namespace Game.Helpers
{

    public class ApplicationDbContext : DbContext
    {
        public string DbPath { get; private set; }
        public DbSet<LeaderboardRecord> Records { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
        }
        
    }
}
