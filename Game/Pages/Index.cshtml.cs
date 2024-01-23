using Game.Models;
using Game.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game.Pages
{
    public class IndexModel(PlayerService ps, ISessionService ss, LocationService ls, List<LeaderboardRecord> leaderboardRecords) : PageModel
    {
        private static readonly string BasicPlayerSessionKey = "PlayerSessionKey";
        private string FullPlayerSessionKey => BasicPlayerSessionKey + ps.UniqueId;

        public List<LeaderboardRecord> LeaderboardRecords { get; set; } = leaderboardRecords;

        public LocationService Ls = ls;

        public IActionResult OnGet()
        {
            LeaderboardRecords = ps.GetTopLeaderboardRecords();
            return Page();
        }

        public IActionResult OnPostStartGame()
        {
            PlayerModel player = ss.GetSession<PlayerModel>(HttpContext, FullPlayerSessionKey);
            Console.WriteLine(FullPlayerSessionKey);

            // inicializuje novou hru a vše vyresetuje
            if (player == null || player.Hp <= 0 || player.CurrentLocationId == -1)
            {
                player = ps.CreateDefaultModel();
                player.CurrentLocationId = 1;
                ss.SaveSession<PlayerModel>(HttpContext, FullPlayerSessionKey, player);
                Ls.ReloadAll();

            }

            return RedirectToPage("Location", new{id = player.CurrentLocationId});
        }
    }
}
