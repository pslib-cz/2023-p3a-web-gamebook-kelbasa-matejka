using System.Reflection.Metadata;
using Game.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game.Pages
{
    public class IndexModel : PageModel
    {
        private static readonly string PLAYER_KEY = "PlayerSessionKey";


        private PlayerService playerService;
        private ISessionService sessionService;
        private LocationService locationService;

        public bool ShowInfo { get; set; }
        
        public IndexModel(PlayerService ps, ISessionService ss, LocationService ls)
        {
            playerService = ps;
            sessionService = ss;
            locationService = ls;

        }

        public void OnGet()
        {
            sessionService.SaveSession<PlayerModel>(HttpContext, PLAYER_KEY, playerService.CreateDefaultModel());
            locationService.ReloadAll();
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
