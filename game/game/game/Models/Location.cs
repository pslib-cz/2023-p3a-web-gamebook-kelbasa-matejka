namespace game.Models;

public class Location
{
    public int LocationID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Background { get; set; }
    public Item Item { get; set; }
    public Effect Effect { get; set; }
    public Enemy Enemy { get; set; }
    public LocationType LocationType { get; set; }
    public List<Action> Actions { get; set; }
    public List<Connection> Connections { get; set; }
}
