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

        public void OnGet()
        {
            ss.SaveSession<PlayerModel>(HttpContext, PLAYER_KEY, ps.CreateDefaultModel()); 
            ls.ReloadAll();  
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
