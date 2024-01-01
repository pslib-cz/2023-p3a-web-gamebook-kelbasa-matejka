using Game.Models;
using System.Text.Json;

namespace Game.Services
{
    public class PlayerService
    {
        private static readonly string DEFAULT_PLAYER_JSON = File.ReadAllText(@"GameData/Player.json");


        public PlayerModel CreateDefaultModel()
        {
            var p = JsonSerializer.Deserialize<PlayerModel>(DEFAULT_PLAYER_JSON);
            p.VisitedConnections = new List<ShortConnection>();
            return p;
        }
        
        public void SaveUsedConnection(PlayerModel p, int from, int to)
        {
            p.VisitedConnections.Add(new ShortConnection() { FromId = from, ToId = to});
        }
        
        public bool ConnectionWasUsed(PlayerModel p, int from, int to)
        {
            int det = p.VisitedConnections.Count(a => a.FromId == from && a.ToId == to);
            Console.WriteLine("Kontroluji");
            foreach(var connection in p.VisitedConnections)
            {
                Console.WriteLine(connection.FromId + " -> " + connection.ToId);
            }
            Console.WriteLine("Počet shod s " + from + " -> " + to + " je " + det);
            return det >= 1;
        }
        
        public void PlayerAttack(PlayerModel p, AttackTypeModel a)
        {
            if (p.Energy >= 15 * (int)a)
            {
                p.CombatState.CurrentEnemyHp = (p.Damage*(int)a >= p.CombatState.CurrentEnemyHp) ? 0 : p.CombatState.CurrentEnemyHp - p.Damage*(int)a;
                p.Energy -= 15 * (int)a;
            }
            if (p.CombatState.CurrentEnemyHp <= 0)
            {
                p.CombatState.CleanedLocations.Add(p.CurrentLocationId);
            }
        
        }
    }
}
