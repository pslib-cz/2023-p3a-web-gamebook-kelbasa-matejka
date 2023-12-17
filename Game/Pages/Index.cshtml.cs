using System.Reflection.Metadata;
using Game.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game.Pages
{
    public class IndexModel : PageModel
    {
        private static readonly string PLAYER_KEY = "PlayerSessionKey";

        private PlayerModel player;
        private PlayerService playerService;
        private ISessionService sessionService;

        public IndexModel(PlayerService ps, ISessionService ss)
        {
            playerService = ps;
            sessionService = ss;

        }

        public void OnGet()
        {
            sessionService.SaveSession<PlayerModel>(PLAYER_KEY, playerService.CreateDefaultModel());
        }
    }
}
