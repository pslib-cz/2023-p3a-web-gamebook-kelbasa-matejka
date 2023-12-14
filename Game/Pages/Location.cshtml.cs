using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Game.Services;
using Microsoft.AspNetCore.Mvc;


namespace Game.Pages;

public class Location : PageModel
{
    private readonly LocationService LocSer;
    
    public Location(LocationService ls)
    {
        LocSer = ls;
    }
    
    public int LocationID { get; private set; } = 1;
    public LocationModel lModel { get; private set; }
    
    
    public void OnGet()
    { 
        lModel = LocSer.GetLocation(LocationID);
    }
}