using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;


namespace Game.Pages;

public class Location : PageModel
{
    private static List<Location> source = new List<Location>();
    static Location()
    {
    
        using (StreamReader r = new StreamReader("../GameData/Locations.json"))  
        {  
            string json = r.ReadToEnd();  
            source = JsonSerializer.Deserialize<List<Location>>(json);  
        }

        foreach (var v in source)
        {
            Console.WriteLine(v.Name);
        }
    } 
    
    Location()
    {
        
    }
    
    public void OnGet()
    {
        
    }
}