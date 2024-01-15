using Game.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game.Pages;

public class Endgame(ISessionService sessionSer, EffectService effSer, PlayerService playerSer)
    : PageModel
{
    private readonly EffectService EffSer = effSer;
    private readonly PlayerService PlayerSer = playerSer;

    private static readonly string PLAYER = "PlayerSessionKey";
    private static readonly int WINNING_LOCATION_ID = 61;

    public PlayerModel pModel { get; set; }
    public bool Win { get; set; }

    public IActionResult OnGet()
    {
        pModel = sessionSer.GetSession<PlayerModel>(HttpContext, PLAYER);

        if(pModel == null || pModel.CurrentLocationId == 0) return RedirectToPage("Index");

        if (pModel.CurrentLocationId == WINNING_LOCATION_ID && pModel.Hp > 0)
        {
            pModel.Won = true;
        }

        if(pModel.Hp <= 0 || pModel.Won)
        {
            pModel.CurrentLocationId = -1;
            sessionSer.SaveSession(HttpContext, PLAYER, pModel);

            Win = pModel.Won;
            return Page();
        }
        else
        {
            return RedirectToPage("Location");
        }


    }
}