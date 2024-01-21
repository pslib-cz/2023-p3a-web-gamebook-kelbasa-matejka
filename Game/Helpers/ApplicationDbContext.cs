using Microsoft.EntityFrameworkCore;
using Game.Models;

namespace Game.Helpers
{

    public class ApplicationDbContext : DbContext // dědí z Microsoft.EntityFrameworkCore.DbContext
    {
        private string _connectionString;
        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
            //Database.EnsureDeleted(); // smazání předchozí databáze
            //Database.EnsureCreated(); // vytvoření nové databáze
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    
        public DbSet<LeaderboardRecord> Records { get; set; } 
    
        protected override void OnModelCreating(ModelBuilder builder) // co se má stát během vytváření modelu
        {
            base.OnModelCreating(builder); // zavolej standardní obsluhu z předka
        }
    }
}
