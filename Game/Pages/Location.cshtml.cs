using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


namespace Game.Pages;

public class Location : PageModel
{
    private static List<LocationModel> source = new List<LocationModel>();
    static Location()
    {
    
        using (StreamReader r = new StreamReader(@"/home/pixel/Documents/2023-p3a-web-gamebook-kelbasa-matejka/Game/GameData/LocationData.json"))
        {  
            string json = r.ReadToEnd();  
            source = JsonSerializer.Deserialize<List<LocationModel>>(json);  
        }

        foreach (var v in source)
        {
            Console.WriteLine(v.Name);
        }
    } 
    
    public int LocationID { get; private set; } = -1;
    public LocationModel lModel { get; private set; }
    
    public void OnGet(int locationID)
    {
        LocationID = locationID;
        Console.WriteLine(LocationID);
        lModel = source.Where(a => a.LocationID == LocationID).FirstOrDefault();
        Console.WriteLine($"LocationID: {lModel.LocationID} Name: {lModel.Name}");
    }
}