namespace game.Models;

public class EffectType
{
    // Enum definition for effect types
    public enum Type
    {
        Open,
        Health,
        Damage,
        Resistance,
        Energy
    }

    public Type EffectTypeValue { get; set; }
}
