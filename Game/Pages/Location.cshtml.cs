using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Game.Services;
using Microsoft.AspNetCore.Mvc;


namespace Game.Pages;

public class Location : PageModel
{
    private readonly LocationService LocSer;
    private readonly ISessionService SessionSer;

    private static readonly string PLAYER = "PlayerSessionKey";

    public Location(LocationService ls, ISessionService ss)
    {
        LocSer = ls;
        SessionSer = ss;
    }
    public LocationModel lModel { get; private set; }
    public PlayerModel pModel { get; private set; }
    
    
    public void OnGet(int id)
    {
        pModel = SessionSer.GetSession<PlayerModel>(PLAYER);
        int last = pModel.CurrentLocationId;
        if (id != 0 && last != 0 && LocSer.ExistsLocation(id))
        {
            if (LocSer.IsNavigationLegitimate(last, id))
            {
                /*
                 * TODO Connection required items check
                 */

                pModel.CurrentLocationId = id;
            }

        }
        /*
         * TODO Connection effects
         */
        SessionSer.SaveSession<PlayerModel>(PLAYER, pModel);
        lModel = LocSer.GetLocation(pModel.CurrentLocationId);
    }
}