namespace game.Models;

public class Connection
{
    public string Description { get; set; }
    public int FromLocationID { get; set; }
    public int ToLocationID { get; set; }
    public Item RequiredItem { get; set; }
    public bool IsLocked { get; set; }
}
