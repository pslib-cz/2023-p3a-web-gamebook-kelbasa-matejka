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

    public bool ExistsLocation(int locationID)
    {
        return Locations.Count(a => a.LocationID == locationID) > 0;
    }
    public bool IsNavigationLegitimate(int fromID, int toID)
    {
        return GetConnections(fromID).Count(a => a.ToLocationID == toID) >= 1;
    }
    public bool IsNavigationLegitimate(int fromID, int toID, PlayerModel p)
    {
        if(p.Hp > 0)
        {
            IsNavigationLegitimate(fromID, toID);
        }
        return false;
    }
}