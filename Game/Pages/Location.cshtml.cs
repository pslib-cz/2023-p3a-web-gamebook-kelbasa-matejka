using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Game.Services;
using Microsoft.AspNetCore.Mvc;
using Game.Models;

namespace Game.Pages;

public class Location(LocationService ls, ISessionService ss, EffectService es, PlayerService ps)
    : PageModel
{
    private static readonly string BASIC_PLAYER_SESSION_KEY = "PlayerSessionKey";

    private string FullPlayerSessionKey
    {
        get
        {
            return BASIC_PLAYER_SESSION_KEY + ps.UniqueId;
        }
    }

    public LocationModel lModel { get; private set; }
    public PlayerModel pModel { get; set; }

    public bool ShowWrongPuzzleGuessMsg { get; set; } = false;

    public IActionResult OnGet(int id)
    {

        //první blok
        LoadPlayer();

        if (pModel == null || pModel.CurrentLocationId == 0)
        {
            Console.WriteLine("Location: " + pModel?.CurrentLocationId);
            return RedirectToPage("Index");
        }

        if (pModel.Items == null || pModel.Hp <= 0)
        {
            Response.Redirect("index");
        }

        int last = pModel.CurrentLocationId;

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

        if (pModel.Hp <= 0 || pModel.CurrentLocationId == -1)
        {
            Response.Redirect("Endgame");
        }

        // druhý blok

        var temp = ls.GetLocation(pModel.CurrentLocationId);
        if (temp == null) lModel = ls.GetLocation(1);
        else lModel = temp;
        
        if(lModel.Enemy != null && !pModel.CombatState.CleanedLocations.Contains(lModel.LocationID) && ! pModel.CombatState.IsCombatActive)
        {
            pModel.CombatState.CurrentEnemy = lModel.Enemy;
        }

        SavePlayer();
        return Page();
    }

    public IActionResult OnPostGuessPuzzle(PuzzleFormModel ffm)
    {
        LoadPlayer();
        lModel = ls.GetLocation(pModel.CurrentLocationId);
        if (ffm == null || lModel.PuzzleKey == null || pModel.SolvedPuzzleLocations.Contains(lModel.LocationID)) return Page();

        ffm.Answer = ffm.Answer.Trim();

        if (ffm.Answer == lModel.PuzzleKey)
        {
            pModel.SolvedPuzzleLocations.Add(lModel.LocationID);
            ShowWrongPuzzleGuessMsg = false;
        }
        else
        {
            EffectService.ApplyEffect(new EffectModel { EffectScale = -5, Type = EffectTypeModel.Health }, pModel);
            ShowWrongPuzzleGuessMsg = true;
        }
        SavePlayer();
        if (pModel.Hp <= 0)
        {
            return RedirectToPage("Endgame");
        }
        return Page();
    }

    /// <summary>
    ///     Item can be picked up only if combat isnt active
    /// </summary>
    /// <param name="ItemID"></param>
    /// <returns></returns>
    public IActionResult OnPostPickUpItem(int ItemID)
    {
        LoadPlayer();
        lModel = ls.GetLocation(pModel.CurrentLocationId);

        if(pModel.CombatState.IsCombatActive) return Page();


        if (pModel.PickedUpItems.Contains(ItemID) || !lModel.Items.Select(a => a.ID).Contains(ItemID))
        {
            return Page();
        }

        var item = lModel.Items.Where(a => a.ID == ItemID).First();

        if (item.IsWearable)
        {
            foreach(var w in pModel.Items.Where(a => a.IsWearable && a.WearableType == item.WearableType))
            {
                var effect = w.OnWearEffect;
                if (effect != null)
                {
                    effect.EffectScale *= -1;
                    EffectService.ApplyEffect(effect, pModel);
                }

            }
            pModel.Items.RemoveAll(a => a.IsWearable && a.WearableType == item.WearableType);

            
            if(item.OnWearEffect != null) EffectService.ApplyEffect(item.OnWearEffect, pModel);
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

        Console.WriteLine("atack se zpracovává");
        if (!pModel.CombatState.IsCombatActive) return Page();
        Console.WriteLine("atack je validní");
        if (attackType == "WeakAttack")
        {
            Console.WriteLine("classic");
            Console.WriteLine("Enemy hp: " + pModel.CombatState.CurrentEnemy.Hp.ToString());
            ps.PlayerAttack(pModel, AttackTypeModel.classic);
        }
        else if (attackType == "StrongAttack")
        {
            if(pModel.Energy < 15) return Page();
            ps.PlayerAttack(pModel, AttackTypeModel.strong);
        }

        Console.WriteLine("Enemy hp: " + pModel.CombatState.CurrentEnemy.Hp.ToString());
        if (pModel.CombatState.CurrentEnemy.Hp > 0)
        {
            EffectService.EnemyAttack(pModel.CombatState.CurrentEnemy, pModel);
        }
        else
        {
            pModel.CombatState.CleanedLocations.Add(pModel.CurrentLocationId);
        }

        SavePlayer();
        if (pModel.Hp <= 0)
        {
            return RedirectToPage("Endgame");
        }

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
        }

        SavePlayer();
        if (pModel.Hp <= 0) return RedirectToPage("Endgame");

        return Page();
    }
    
    /// <summary>
    ///     Saves player model to session
    /// </summary>
    public void SavePlayer()
    {
        ss.SaveSession<PlayerModel>(HttpContext, FullPlayerSessionKey, pModel);
    }


    /// <summary>
    ///     Loads player model from session to <see cref="pModel"/>
    /// </summary>
    public void LoadPlayer()
    {
        pModel = ss.GetSession<PlayerModel>(HttpContext, FullPlayerSessionKey);
    }

}