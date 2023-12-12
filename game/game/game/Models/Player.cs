namespace game.Models;

public class Player
{
    public int Id { get; set; }
    public int Hp { get; set; }
    public Inventory Inventory { get; set; }
    public int Damage { get; set; }
    public int Resistance { get; set; }
    public int Stage { get; set; }
}