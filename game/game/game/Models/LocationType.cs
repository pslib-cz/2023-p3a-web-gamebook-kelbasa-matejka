namespace game.Models;

public class LocationType
{
    // Enum definition for location types
    public enum Type
    {
        Dungeon,
        Loot,
        Trap,
        Cross
    }

    public Type LocationTypeValue { get; set; }
}
