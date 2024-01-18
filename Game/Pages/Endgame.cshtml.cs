using Game.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game.Pages;

public class Endgame(ISessionService sessionSer, EffectService effSer, PlayerService playerSer)
    : PageModel
{
    private readonly EffectService EffSer = effSer;
    private readonly PlayerService PlayerSer = playerSer;

    private static readonly string BASIC_PLAYER_SESSION_KEY = "PlayerSessionKey";
    private string FullPlayerSessionKey
    {
        get
        {
            return BASIC_PLAYER_SESSION_KEY + playerSer.UniqueId;
        }
    }

    public DateTime? Time
    {
        get
        {
            if (pModel.Won) return new DateTime(pModel.FinishedAt.Ticks - pModel.CreatedAt.Ticks);
            else return null;
        }
    }


    public PlayerModel pModel { get; set; }
    public bool Win { get; set; }

    public IActionResult OnGet()
    {
        pModel = sessionSer.GetSession<PlayerModel>(HttpContext, FullPlayerSessionKey);

        if(pModel == null || pModel.CurrentLocationId == 0) return RedirectToPage("Index");

        if (pModel.CurrentLocationId == pModel.WinningLocationId && pModel.Hp > 0)
        {
            pModel.FinishedAt = DateTime.Now;
            pModel.Won = true;
        }

        if(pModel.Hp <= 0 || pModel.Won)
        {
            pModel.CurrentLocationId = -1;
            sessionSer.SaveSession(HttpContext, FullPlayerSessionKey, pModel);

            Win = pModel.Won;
            return Page();
        }
        else
        {
            return RedirectToPage("Location");
        }


    }
}