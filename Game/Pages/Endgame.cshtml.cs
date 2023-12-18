using Game.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game.Pages;

public class Endgame : PageModel
{
    private readonly ISessionService SessionSer;
    private readonly EffectService EffSer;
    private readonly PlayerService PlayerSer;

    private static readonly string PLAYER = "PlayerSessionKey";
    private static readonly int WINNING_LOCATION_ID = 3;

    public PlayerModel pModel;
    public bool Win {  get; set; }
    public Endgame(ISessionService sessionSer, EffectService effSer, PlayerService playerSer)
    {
        SessionSer = sessionSer;
        EffSer = effSer;
        PlayerSer = playerSer;
    }

    public void OnGet()
    {
        pModel = SessionSer.GetSession<PlayerModel>(PLAYER);
        if(pModel.CurrentLocationId == WINNING_LOCATION_ID && pModel.Hp > 0)
        {
            Win = true;
        }else if(pModel.CurrentLocationId != WINNING_LOCATION_ID && pModel.Hp > 0)
        {
            Response.Redirect("Index");
            Win = false;
        }
        else
        {
            Win = false;
        }
        pModel.CurrentLocationId = -1;
        SessionSer.SaveSession(PLAYER, pModel);
    }
}