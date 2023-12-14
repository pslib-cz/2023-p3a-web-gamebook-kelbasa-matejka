using System.Text.Json;

namespace Game.Services;

public class LocationService
{
    public List<LocationModel> Locations { get; private set; }
    public List<ConnectionModel> Connections { get; private set; }
    
    public LocationService()
    {
        Locations = JsonSerializer.Deserialize<List<LocationModel>>(File.ReadAllText(@"GameData/LocationData.json"));
        Connections = Locations.Select(a => a.Connections).SelectMany(a => a).ToList();
        
    }
    
    public LocationModel? GetLocation(int locationID)
    {
        return Locations.Where(a => a.LocationID == locationID).FirstOrDefault();
    }
    
    public List<ConnectionModel> GetConnections(int locationID)
    {
        return Connections.Where(a => a.FromLocationID == locationID).ToList();
    }
}