using System.ComponentModel.DataAnnotations;

namespace Game.Models
{
    public class LeaderboardRecord
    {
        [Key]
        public string PlayerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime PlayTime { get; set; }
        [Required]
        public DateTime SavedAt { get; set; }

    }
}
