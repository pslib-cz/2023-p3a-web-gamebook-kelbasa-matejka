using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Game.Services;
using Microsoft.AspNetCore.Mvc;
using Game.Models;

namespace Game.Pages;

public class Location(LocationService ls, ISessionService ss, EffectService es, PlayerService ps)
    : PageModel
{
    private static readonly string PLAYER = "PlayerSessionKey";
    private static readonly int WINNING_LOCATION_ID = 3;

    public LocationModel lModel { get; private set; }
    public PlayerModel pModel { get; set; }

    public void OnGet(int id)
    {
        //SESSION LOAD
        pModel = ss.GetSession<PlayerModel>(HttpContext, PLAYER);


        int last = pModel.CurrentLocationId;

        if (pModel.CurrentLocationId == WINNING_LOCATION_ID && id == -1)
        {
            Response.Redirect("Endgame");
        }

        if (id > 0 && last > 0 && ls.ExistsLocation(id))
        {
            if (ls.IsNavigationLegitimate(last, id, pModel))
            {
                var con = ls.GetConnection(last, id);
                if(pModel.CombatState.CurrentEnemyHp == 0 && lModel.Enemy != null)
                {
                    pModel.CombatState.CurrentEnemyHp = lModel.Enemy.Hp;
                }
                
                if (con.Effect != null && !ps.ConnectionWasUsed(pModel, con.FromLocationID, con.ToLocationID))
                {
                    es.ApplyEffect(con.Effect, pModel);
                    ps.SaveUsedConnection(pModel, con.FromLocationID, con.ToLocationID);

                }
                pModel.CurrentLocationId = id;
            }

        }
        if (pModel.Hp <= 0 || pModel.CurrentLocationId == -1)
        {
            Response.Redirect("Endgame");
        }
        ss.SaveSession<PlayerModel>(HttpContext, PLAYER, pModel);
        var temp = ls.GetLocation(pModel.CurrentLocationId);
        if (temp == null) lModel = ls.GetLocation(1);
        else lModel = temp;


    }

    // prozatím pouze kontrola hádanky
    public IActionResult OnPostGuessPuzzle(LocationFormModel ffm)
    {
        OnGet(0);
        if (ffm == null || lModel.PuzzleKey == null) return Page();
        if (ffm.Answer == lModel.PuzzleKey)
        {
            ls.SolvedPuzzle(lModel.LocationID);
        }
        return Page();
    }
    
    // probehne kolo utoku mezi hracem a enemakem
    public IActionResult OnPostAttack(string attackType)
    {
        if (attackType == "WeakAttack")
        {
            
        }
        else if (attackType == "StrongAttack")
        {
            // Logika pro silný útok
        }

        return Page();
    }

    // Metoda pro použití pøedmìtu
    public IActionResult OnPostUseItem()
    {
        return Page();
    }

}