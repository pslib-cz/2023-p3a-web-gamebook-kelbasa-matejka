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
            return JsonSerializer.Deserialize<PlayerModel>(DEFAULT_PLAYER_JSON);
        }
    }
}
