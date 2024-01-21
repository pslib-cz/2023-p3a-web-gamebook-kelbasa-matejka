using Game.Helpers;
using Game.Models;
using System.Text.Json;

namespace Game.Services
{
    public class PlayerService
    {
        private static readonly string DEFAULT_PLAYER_JSON = File.ReadAllText(@"GameData/Player.json");
        private ApplicationDbContext db;

        public string UniqueId { get; private set; }

        public PlayerService() 
        {
            Console.WriteLine("Jsem tvořený");
            db = new ApplicationDbContext(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GamebookDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            UniqueId = Guid.NewGuid().ToString();
        }


        public PlayerModel CreateDefaultModel()
        {
            var p = JsonSerializer.Deserialize<PlayerModel>(DEFAULT_PLAYER_JSON);
            if (p == null) throw new JsonException();
            p.VisitedConnections = new List<ShortConnection>();
            p.CreatedAt = DateTime.Now;
            p.PlayerID = Guid.NewGuid().ToString();

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

        public bool PutIntoLeaderboard(PlayerModel p, string name)
        {
            if (db.Records.Any(r => r.PlayerId == p.PlayerID)) return false;


            var playTime = new DateTime(p.FinishedAt.Ticks - p.CreatedAt.Ticks);
            db.Records.Add(new LeaderboardRecord { Name = name, PlayerId = p.PlayerID, PlayTime = playTime, SavedAt = DateTime.Now });
            db.SaveChanges();

            return true;
            
        }

        public List<LeaderboardRecord> GetTopLeaderboardRecords()
        {
            return db.Records.OrderByDescending(r => r.PlayTime.Millisecond).Take(10).ToList();
        }


    }
}
