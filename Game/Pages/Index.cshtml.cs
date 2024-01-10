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
            
            // Pokud session existuje a hr�� je �iv� a nen� na startovn�m m�st�, p�esm�ruj na Location
            if (player.Hp > 0 && player.CurrentLocationId != 0)
            {
                return RedirectToPage("/Location", new { id = player.CurrentLocationId });
            }

            // Vytvo�� nov�ho hr��e a ulo� ho do session
            var newPlayer = ps.CreateDefaultModel();
            ss.SaveSession(HttpContext, PLAYER_KEY, newPlayer);
            ls.ReloadAll();

            return Page();
        }
    }
}
