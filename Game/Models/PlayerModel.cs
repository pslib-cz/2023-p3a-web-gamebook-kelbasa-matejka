using Game.Models;

namespace Game {
    public class PlayerModel {
        public string PlayerID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public bool SavedToLeaderboard { get; set; } = false;
        public int Hp { get; set; }
        public List<ItemModel> Items { get; set; }
        public int CurrentLocationId { get; set; }
        public int Damage { get; set; }
        public int Energy { get; set; }
        public int Resistance { get; set; }
        public List<ShortConnection> VisitedConnections { get; set; }
        public HashSet<int> SolvedPuzzleLocations { get; set; } = new HashSet<int>();
        public HashSet<int> PickedUpItems { get; set; } = new HashSet<int>();
        public CombatStateModel CombatState { get; set; }
        public bool Won { get; set; } = false;
        public int WinningLocationId {  get; set; }
    }
}
