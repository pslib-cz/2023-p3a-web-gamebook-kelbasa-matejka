namespace game.Models;

public class Effect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public EffectType Type { get; set; }
    public int? EffectScale { get; set; }
}
