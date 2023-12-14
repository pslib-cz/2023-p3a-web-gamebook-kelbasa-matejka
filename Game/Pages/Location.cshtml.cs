using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Game.Services;
using Microsoft.AspNetCore.Mvc;


namespace Game.Pages;

public class Location : PageModel
{
    private readonly LocationService LocSer;
    private readonly ISessionService SessionSer;

    private static readonly string SESSION_LASTID_KEY = "LastVisitedLocationId";
    
    public Location(LocationService ls, ISessionService ss)
    {
        LocSer = ls;
        SessionSer = ss;
    }
    
    public int LocationID { get; private set; } = 1;
    public LocationModel lModel { get; private set; }
    
    
    public void OnGet(int id)
    {
        int last = SessionSer.GetSession<int>(SESSION_LASTID_KEY);
        if (id != 0 && last != 0 && LocSer.ExistsLocation(id))
        {
            if (LocSer.IsNavigationLegitimate(last, id))
            {
                LocationID = id;
            }
            else
            {
                LocationID = last;
            }

        }
        SessionSer.SaveSession(SESSION_LASTID_KEY, LocationID);
        lModel = LocSer.GetLocation(LocationID);
    }
}