using System.Numerics;
using System.Reflection.Metadata;
using Game.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game.Pages
{
    public class IndexModel(PlayerService ps, ISessionService ss, LocationService ls) : PageModel
    {
        private static readonly string PLAYER_KEY = "PlayerSessionKey";

        public bool ShowInfo { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostStartGame()
        {
            PlayerModel player = ss.GetSession<PlayerModel>(HttpContext, PLAYER_KEY);

            // inicializuje novou hru a vše vyresetuje
            if (player == null || player.Hp <= 0 || player.CurrentLocationId == -1)
            {
                player = ps.CreateDefaultModel();
                player.CurrentLocationId = 1;
                ss.SaveSession<PlayerModel>(HttpContext, PLAYER_KEY, player);
                ls.ReloadAll();

            }

            return RedirectToPage("Location", new{id = player.CurrentLocationId});
        }

        public IActionResult OnPostShowInfo()
        {
            ShowInfo = true;
            return Page();
        }

        public IActionResult OnPostHideInfo()
        {
            ShowInfo = false;
            return Page();
        }
    }
}
