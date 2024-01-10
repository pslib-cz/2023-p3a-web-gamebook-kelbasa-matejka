using Game.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game.Pages;

public class Endgame(ISessionService sessionSer, EffectService effSer, PlayerService playerSer)
    : PageModel
{
    private readonly EffectService EffSer = effSer;
    private readonly PlayerService PlayerSer = playerSer;

    private static readonly string PLAYER = "PlayerSessionKey";
    private static readonly int WINNING_LOCATION_ID = 8;

    public PlayerModel pModel;
    public bool Win { get; set; }

    public void OnGet()
    {
        pModel = sessionSer.GetSession<PlayerModel>(HttpContext, PLAYER);
        if(pModel == null || pModel.CurrentLocationId == 0) Response.Redirect("/");
        else
        {
            if (pModel.CurrentLocationId == WINNING_LOCATION_ID && pModel.Hp > 0)
            {
                pModel.Won = true;
            }
            pModel.CurrentLocationId = -1;
            sessionSer.SaveSession(HttpContext, PLAYER, pModel);

            Win = pModel.Won;
        }

    }
}