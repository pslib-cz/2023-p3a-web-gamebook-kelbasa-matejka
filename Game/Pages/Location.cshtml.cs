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
    private static readonly int WINNING_LOCATION_ID = 1000;

    public LocationModel lModel { get; private set; }
    public PlayerModel pModel { get; set; }

    public void OnGet(int id)
    {

        //první blok
        LoadPlayer();

        int last = pModel.CurrentLocationId;

        if (pModel.CurrentLocationId == WINNING_LOCATION_ID && id == -1)
        {
            Response.Redirect("Endgame");
        }

        if (id > 0 && last > 0 && ls.ExistsLocation(id))
        {
            if (ls.IsNavigationLegitimate(last, id, pModel) && !pModel.CombatState.IsCombatActive)
            {
                var con = ls.GetConnection(last, id);
                
                if (!ps.ConnectionWasUsed(pModel, con.FromLocationID, con.ToLocationID))
                {
                    if (con.Effect != null )
                    {
                        EffectService.ApplyEffect(con.Effect, pModel);    
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


        // druhý blok

        var temp = ls.GetLocation(pModel.CurrentLocationId);
        if (temp == null) lModel = ls.GetLocation(1);
        else lModel = temp;
        
        
        if(lModel.Enemy != null && !pModel.CombatState.CleanedLocations.Contains(lModel.LocationID) && ! pModel.CombatState.IsCombatActive)
        {
            pModel.CombatState.CurrentEnemy = lModel.Enemy;
            SavePlayer();
        }
    }

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
            EffectService.ApplyEffect(new EffectModel { EffectScale = -10, Type = EffectTypeModel.Health }, pModel);
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

        var item = lModel.Items.Where(a => a.ID == ItemID).First();

        if (item.IsWearable)
        {
            foreach(var w in pModel.Items.Where(a => a.IsWearable && a.WearableType == item.WearableType))
            {
                var effect = w.OnWearEffect;
                effect.EffectScale *= -1;
                EffectService.ApplyEffect(effect, pModel);
            }
            pModel.Items.RemoveAll(a => a.IsWearable && a.WearableType == item.WearableType);

            EffectService.ApplyEffect(item.OnWearEffect, pModel);
        }

        pModel.Items.Add(item);
        pModel.PickedUpItems.Add(ItemID);
        SavePlayer();
        return Page();
    }


    // probehne kolo utoku mezi hracem a enemakem
    public IActionResult OnPostAttack(string attackType)
    {
        LoadPlayer();
        lModel = ls.GetLocation(pModel.CurrentLocationId);

        if (attackType == "WeakAttack")
        {
            ps.PlayerAttack(pModel, AttackTypeModel.classic);
        }
        else if (attackType == "StrongAttack")
        {
            ps.PlayerAttack(pModel, AttackTypeModel.strong);
        }

        if(pModel.CombatState.CurrentEnemy.Hp > 0)
        {
            EffectService.EnemyAttack(pModel.CombatState.CurrentEnemy, pModel);
        }
        else
        {
            pModel.CombatState.CleanedLocations.Add(pModel.CurrentLocationId);
        }

        if (pModel.Hp <= 0)
        {
            return RedirectToPage("Endgame");
        }
        SavePlayer();
        return Page();
    }

    // probehne pouziti itemu
    public IActionResult OnPostUseItem(int ItemId)
    {
        LoadPlayer();
        lModel = ls.GetLocation(pModel.CurrentLocationId);
        if (pModel.Items.Count(a => a.ID == ItemId) == 0) return Page();
        var item = pModel.Items.Where(a => a.ID == ItemId).First();
        pModel.Items.Remove(item);
        if(item.TargetIsEnemy)
        {
            if(pModel.CombatState.IsCombatActive)
            {
                EffectService.ApplyEffect(item.OnUseEffect, pModel.CombatState.CurrentEnemy);
                if (pModel.CombatState.CurrentEnemy.Hp <= 0)
                {
                    pModel.CombatState.CleanedLocations.Add(pModel.CurrentLocationId);
                }
            }
        }
        else
        {
            EffectService.ApplyEffect(item.OnUseEffect, pModel);
            if (pModel.Hp <= 0) return RedirectToPage("Endgame");
        }

        SavePlayer();
        return Page();
    }
    
    /// <summary>
    ///     Saves player model to session
    /// </summary>
    public void SavePlayer()
    {
        ss.SaveSession<PlayerModel>(HttpContext, PLAYER, pModel);
    }


    /// <summary>
    ///     Loads player model from session
    /// </summary>
    public void LoadPlayer()
    {
        pModel = ss.GetSession<PlayerModel>(HttpContext, PLAYER);
    }

}