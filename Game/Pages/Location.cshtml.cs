using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Game.Services;
using Microsoft.AspNetCore.Mvc;
using Game.Models;

namespace Game.Pages;

public class Location : PageModel
{
    private readonly LocationService LocSer;
    private readonly ISessionService SessionSer;
    private readonly EffectService EffSer;
    private readonly PlayerService PlayerSer;

    private static readonly string PLAYER = "PlayerSessionKey";
    private static readonly int WINNING_LOCATION_ID = 3;
    
    public Location(LocationService ls, ISessionService ss, EffectService es, PlayerService ps)
    {
        LocSer = ls;
        SessionSer = ss;
        EffSer = es;
        PlayerSer = ps;     
    }
    public LocationModel lModel { get; private set; }
    public PlayerModel pModel { get; set; }




    public void OnGet(int id)
    {
        //SESSION LOAD
        pModel = SessionSer.GetSession<PlayerModel>(HttpContext, PLAYER);


        int last = pModel.CurrentLocationId;

        if (pModel.CurrentLocationId == WINNING_LOCATION_ID && id == -1)
        {
            Response.Redirect("Endgame");
        }

        if (id > 0 && last > 0 && LocSer.ExistsLocation(id))
        {
            if (LocSer.IsNavigationLegitimate(last, id, pModel))
            {
                var con = LocSer.GetConnection(last, id);
                ;
                if (con.Effect != null && !PlayerSer.ConnectionWasUsed(pModel, con.FromLocationID, con.ToLocationID))
                {
                    EffSer.ApplyEffect(con.Effect, pModel);
                    PlayerSer.SaveUsedConnection(pModel, con.FromLocationID, con.ToLocationID);

                }
                pModel.CurrentLocationId = id;
            }

        }
        if (pModel.Hp <= 0 || pModel.CurrentLocationId == -1)
        {
            Response.Redirect("Endgame");
        }
        SessionSer.SaveSession<PlayerModel>(HttpContext, PLAYER, pModel);
        var temp = LocSer.GetLocation(pModel.CurrentLocationId);
        if (temp == null) lModel = LocSer.GetLocation(1);
        else lModel = temp;


    }

    // prozatím pouze kontrola hádanky
    public IActionResult OnPostGuessPuzzle(LocationFormModel ffm)
    {
        OnGet(0);
        if (ffm == null || lModel.PuzzleKey == null) return Page();
        if (ffm.Answer == lModel.PuzzleKey)
        {
            LocSer.SolvedPuzzle(lModel.LocationID);
        }
        return Page();
    }

}