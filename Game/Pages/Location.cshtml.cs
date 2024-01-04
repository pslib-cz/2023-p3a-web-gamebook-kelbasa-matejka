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
        LoadPlayer();

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
        SavePlayer();
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

    // prozat�m pouze kontrola h�danky
    public IActionResult OnPostGuessPuzzle(PuzzleFormModel ffm)
    {
        LoadPlayer();
        lModel = ls.GetLocation(pModel.CurrentLocationId);
        if (ffm == null || lModel.PuzzleKey == null || pModel.SolvedPuzzleLocations.Contains(lModel.LocationID)) return Page();
        if (ffm.Answer == lModel.PuzzleKey)
        {
            pModel.SolvedPuzzleLocations.Add(lModel.LocationID);
            ls.SolvedPuzzle(lModel.LocationID);
        }
        else
        {
            es.ApplyEffect(new EffectModel { EffectScale = -10, Type = EffectTypeModel.Health }, pModel);
        }
        SavePlayer();
        return Page();
    }


    public IActionResult OnPostPickUpItem(int ItemID)
    {
        LoadPlayer();
        lModel = ls.GetLocation(pModel.CurrentLocationId);

        if(pModel.PickedUpItems.Contains(ItemID) || !lModel.Items.Select(a => a.ID).Contains(ItemID))
        {
            return Page();
        }

        pModel.Items.Add(lModel.Items.Where(a => a.ID == ItemID).First());
        pModel.PickedUpItems.Add(ItemID);
        SavePlayer();
        return Page();
    }


    // probehne kolo utoku mezi hracem a enemakem
    public IActionResult OnPostAttack(string attackType)
    {
        LoadPlayer();
        lModel = ls.GetLocation(pModel.CurrentLocationId);
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

    // Metoda pro pou�it� p�edm�tu
    public IActionResult OnPostUseItem()
    {
        OnGet(0);
        return Page();
    }
    
    public void SavePlayer()
    {
        ss.SaveSession<PlayerModel>(HttpContext, PLAYER, pModel);
    }

    public void LoadPlayer()
    {
        pModel = ss.GetSession<PlayerModel>(HttpContext, PLAYER);
    }

}