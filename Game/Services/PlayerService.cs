using System.Text.Json;

namespace Game.Services
{
    public class PlayerService
    {
        private static readonly string DEFAULT_PLAYER_JSON = File.ReadAllText(@"GameData/Player.json");

        public PlayerService() { 
        
        }

        public PlayerModel CreateDefaultModel()
        {
            var p =JsonSerializer.Deserialize<PlayerModel>(DEFAULT_PLAYER_JSON);
            p.VisitedConnections = "";
            return p;
        }
        
        public void SaveUsedConnection(PlayerModel p, int from, int to)
        {
            p.VisitedConnections += "(" + from + ";" + to + ")";
        }
        
        public bool ConnectionWasUsed(PlayerModel p, int from, int to)
        {
            return p.VisitedConnections.Contains("(" + from + ";" + to + ")");
        }
    }
}
