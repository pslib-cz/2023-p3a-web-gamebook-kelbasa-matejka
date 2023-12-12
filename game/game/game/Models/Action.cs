namespace game.Models;

public class Action
{
    public string Description { get; set; }
    public string Icon { get; set; }
    public Effect Effect { get; set; }
    public int EnergyCost { get; set; }
}
