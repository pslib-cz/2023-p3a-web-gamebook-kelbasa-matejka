using System.Text.Json;
using Game.Models;

namespace Game.Services;

public class LocationService
{
    public List<LocationModel> Locations { get; private set; }
    public List<ConnectionModel> Connections { get; private set; }
    
    public LocationService()
    {   
        ReloadAll();
    }

    public void ReloadAll()
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

    public ConnectionModel GetConnection(int fromID, int toID)
    {
        return GetConnections(fromID).Where(a => a.ToLocationID == toID).FirstOrDefault();
    }

    public bool ExistsLocation(int locationID)
    {
        return Locations.Count(a => a.LocationID == locationID) > 0;
    }
    public bool IsNavigationLegitimate(int fromID, int toID)
    {
        return GetConnections(fromID).Count(a => a.ToLocationID == toID) >= 1;
    }

    //check for required items
    public bool IsNavigationLegitimate(int fromID, int toID, PlayerModel p)
    {
        if(p.Hp > 0)
        {
            if(IsNavigationLegitimate(fromID, toID))
            {
                var connection = GetConnection(fromID, toID);
                if (connection.Locked)
                {
                    if(!p.SolvedPuzzleLocations.Contains(fromID)) return false;
                }
                if (connection.RequiredItem == null) return true;
                if (p.Items.Count(a => a.ID == connection.RequiredItem?.ID) > 0) return true;
            }

        }
        return false;
    }
    
    public void SolvedPuzzle(int fromID)
    {
        for(int i = 0; i < Connections.Count; i++)
        {
            if (Connections[i].FromLocationID == fromID) Connections[i].Locked = false;
        }
    }   
    
}