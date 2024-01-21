using System.Numerics;
using System.Reflection.Metadata;
using Game.Models;
using Game.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game.Pages
{
    public class IndexModel : PageModel
    {
        private static readonly string BASIC_PLAYER_SESSION_KEY = "PlayerSessionKey";
        private string FullPlayerSessionKey
        {
            get
            {
                return BASIC_PLAYER_SESSION_KEY + ps.UniqueId;
            }
        }

        public List<LeaderboardRecord> LeaderboardRecords { get; set; }

        public IndexModel(PlayerService ps, ISessionService ss, LocationService ls)
        {
            this.ps = ps;
            this.ss = ss;
            this.ls = ls;
        }

        private PlayerService ps;
        private ISessionService ss;
        public LocationService ls;

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
                ls.ReloadAll();

            }

            return RedirectToPage("Location", new{id = player.CurrentLocationId});
        }
    }
}
