using Game.Models;
using System.Text.Json;

namespace Game.Services
{
    public class PlayerService
    {
        private static readonly string DEFAULT_PLAYER_JSON = File.ReadAllText(@"GameData/Player.json");


        public string UniqueId { get; private set; }

        public PlayerService() 
        {
            Console.WriteLine("Jsem tvořený");
            UniqueId = Guid.NewGuid().ToString();
        }


        public PlayerModel CreateDefaultModel()
        {
            var p = JsonSerializer.Deserialize<PlayerModel>(DEFAULT_PLAYER_JSON);
            if (p == null) throw new JsonException();
            p.VisitedConnections = new List<ShortConnection>();
            p.CreatedAt = DateTime.Now;

            foreach(var item in p.Items.Where(a => a.IsWearable).ToArray())
            {
                EffectService.ApplyEffect(item.OnWearEffect, p);
            }
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
            double randomizer = 0.7 + (Random.Shared.NextDouble() * 0.5);
            int calculatedAttack = -1 * (int)Math.Floor(((double) p.Damage) * randomizer);

            if (a == AttackTypeModel.classic)
            {
                EffectService.ApplyEffect(new EffectModel { EffectScale = calculatedAttack, Type = EffectTypeModel.Health }, p.CombatState.CurrentEnemy);
            }
            else if(a == AttackTypeModel.strong && p.Energy >= 15)
            {
                calculatedAttack = (int) (calculatedAttack * 1.8);
                EffectService.ApplyEffect(new EffectModel { EffectScale = calculatedAttack, Type = EffectTypeModel.Health }, p.CombatState.CurrentEnemy);
                EffectService.ApplyEffect(new EffectModel { EffectScale = -15, Type = EffectTypeModel.Energy }, p);
            }
            if (p.CombatState.CurrentEnemy.Hp <= 0)
            {
                p.CombatState.CleanedLocations.Add(p.CurrentLocationId);
            }
        
        }
    }
}
