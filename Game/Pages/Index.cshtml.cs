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
            PlayerModel player = ss.GetSession<PlayerModel>(HttpContext, PLAYER_KEY);

            if (player.Hp > 0 && player.CurrentLocationId != -1)
            {
                return RedirectToPage("/Location", new { id = player.CurrentLocationId });
            }

            // Create a new player session and save it
            var newPlayer = ps.CreateDefaultModel();
            ss.SaveSession(HttpContext, PLAYER_KEY, newPlayer);
            ls.ReloadAll();

            return Page();
        }

        public IActionResult OnPostShowInfo()
        {
            ShowInfo = true;
            Console.WriteLine("show info is" + ShowInfo);
            return Page();
        }

        public IActionResult OnPostHideInfo()
        {
            ShowInfo = false;
            return Page();
        }
    }
}
