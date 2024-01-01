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
            if (ls.IsNavigationLegitimate(last, id, pModel) && pModel.CombatState.CurrentEnemyHp == 0)
            {
                var con = ls.GetConnection(last, id);
                
                if (!ps.ConnectionWasUsed(pModel, con.FromLocationID, con.ToLocationID))
                {
                    if (con.Effect != null )
                    {
                        es.ApplyEffect(con.Effect, pModel);    
                    }
                    
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
        if(lModel.Enemy != null && pModel.CombatState.CurrentEnemyHp == 0 && !pModel.CombatState.CleanedLocations.Contains(lModel.LocationID))
        {
            pModel.CombatState.CurrentEnemyHp = lModel.Enemy.Hp;
            SavePlayer();
        }
        if (lModel.Enemy != null && lModel.Enemy.Hp <= 0)
        {
            pModel.CombatState.CleanedLocations.Add(lModel.LocationID);
            SavePlayer();
        }
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
        else
        {
            pModel.Hp -= 10;
            SavePlayer();
        }
        return Page();
    }
    
    // probehne kolo utoku mezi hracem a enemakem
    public IActionResult OnPostAttack(string attackType)
    {
        OnGet(0);
        if (attackType == "WeakAttack" && pModel.Energy >= 15)
        {
            ps.PlayerAttack(pModel, AttackTypeModel.classic);
        }
        else if (attackType == "StrongAttack" && pModel.Energy >= 15)
        {
            ps.PlayerAttack(pModel, AttackTypeModel.strong);
        }
        LocationService.EnemyAttack(lModel.Enemy, pModel);
        SavePlayer();
        return Page();
    }

    // Metoda pro použití pøedmìtu
    public IActionResult OnPostUseItem()
    {
        OnGet(0);
        return Page();
    }
    
    public void SavePlayer()
    {
        ss.SaveSession<PlayerModel>(HttpContext, PLAYER, pModel);
    }

}