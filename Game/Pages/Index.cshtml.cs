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
            
            // Pokud session existuje a hráè je živý a není na startovním místì, pøesmìruj na Location
            if (player.Hp > 0 && player.CurrentLocationId != 0)
            {
                return RedirectToPage("/Location", new { id = player.CurrentLocationId });
            }

            // Vytvoøí nového hráèe a ulož ho do session
            var newPlayer = ps.CreateDefaultModel();
            ss.SaveSession(HttpContext, PLAYER_KEY, newPlayer);
            ls.ReloadAll();

            return Page();
        }
    }
}
