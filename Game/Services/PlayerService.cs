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
            foreach(var item in p.Items)
            {
                if(item.IsWearable)
                {
                    EffectService.ApplyEffect(item.OnWearEffect, p);
                }
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
            if (a == AttackTypeModel.classic && p.Energy >= 5)
            {
                EffectService.ApplyEffect(new EffectModel { EffectScale = -1*p.Damage, Type = EffectTypeModel.Health }, p.CombatState.CurrentEnemy);
                EffectService.ApplyEffect(new EffectModel { EffectScale = -5, Type = EffectTypeModel.Energy }, p);
            }
            else if(a == AttackTypeModel.strong && p.Energy >= 15)
            {
                EffectService.ApplyEffect(new EffectModel { EffectScale = -2 * p.Damage, Type = EffectTypeModel.Health }, p.CombatState.CurrentEnemy);
                EffectService.ApplyEffect(new EffectModel { EffectScale = -15, Type = EffectTypeModel.Energy }, p);
            }
            if (p.CombatState.CurrentEnemy.Hp <= 0)
            {
                p.CombatState.CleanedLocations.Add(p.CurrentLocationId);
            }
        
        }
    }
}
