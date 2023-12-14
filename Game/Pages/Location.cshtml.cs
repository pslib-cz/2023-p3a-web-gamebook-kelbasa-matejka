using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Game.Services;
using Microsoft.AspNetCore.Mvc;


namespace Game.Pages;

public class Location : PageModel
{
    private readonly LocationService LocSer;
    private readonly ISessionService<int> SessionSer;
    
    public Location(LocationService ls, ISessionService<int> ss)
    {
        LocSer = ls;
        SessionSer = ss;
    }
    
    public int LocationID { get; private set; } = 1;
    public LocationModel lModel { get; private set; }
    
    
    public void OnGet(int id)
    {
        
        Console.WriteLine(id);
        int last = SessionSer.GetSession<int>("Current");
        Console.WriteLine("Session" + last);
        if (id != 0 && last != 0 && LocSer.ExistsLocation(id))
        {

            var check = LocSer.GetConnections(last).Select(a => a.ToLocationID).ToArray();
            foreach(var i in check)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("Hledám " + id);
            if (check.Contains(id))
            {
                LocationID = id;
            }

        }
        SessionSer.SaveSession("Current", LocationID);
        lModel = LocSer.GetLocation(LocationID);
    }
}