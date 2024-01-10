using Game.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game.Pages;

public class Endgame(ISessionService sessionSer, EffectService effSer, PlayerService playerSer)
    : PageModel
{
    private readonly EffectService EffSer = effSer;
    private readonly PlayerService PlayerSer = playerSer;

    private static readonly string PLAYER = "PlayerSessionKey";
    private static readonly int WINNING_LOCATION_ID = 3;

    public PlayerModel pModel;
    public bool Win {  get; set; }

    public void OnGet()
    {
        pModel = sessionSer.GetSession<PlayerModel>(HttpContext, PLAYER);
        if (pModel.CurrentLocationId == 0)
        {
            Response.Redirect("/");
        }
        if(pModel.CurrentLocationId == WINNING_LOCATION_ID && pModel.Hp > 0)
        {
            Win = true;
        }
        else
        {
            Win = false;
        }
        pModel.CurrentLocationId = 0;
        sessionSer.SaveSession(HttpContext, PLAYER, pModel);
    }
}