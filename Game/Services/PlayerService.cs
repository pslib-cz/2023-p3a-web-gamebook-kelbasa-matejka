using Game.Helpers;
using Game.Models;
using System.Text.Json;

namespace Game.Services
{
    public class PlayerService
    {
        private static readonly string DEFAULT_PLAYER_JSON = File.ReadAllText(@"GameData/Player.json");
        private ApplicationDbContext _context;


        public string UniqueId { get; private set; }

        public PlayerService(ApplicationDbContext context) 
        {
            Console.WriteLine("PlayerService starting...");
            UniqueId = Guid.NewGuid().ToString();
            _context = context;
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
            if (_context.Records.Any(r => r.PlayerId == p.PlayerID)) return false;


            var playTime = new DateTime(p.FinishedAt.Ticks - p.CreatedAt.Ticks);
            _context.Records.Add(new LeaderboardRecord { Name = name, PlayerId = p.PlayerID, PlayTime = playTime, SavedAt = DateTime.Now });
            _context.SaveChanges();

            return true;
            
        }

        public List<LeaderboardRecord> GetTopLeaderboardRecords()
        {
            Console.WriteLine("Getting top leaderboard records...");
            var data = _context.Records.OrderBy(r => r.PlayTime).Take(10).ToList();
            if (data == null || data.Count == 0)
            {
                return new List<LeaderboardRecord>();
            }
            return _context.Records.OrderBy(r => r.PlayTime).Take(10).ToList();
        }


    }
}
