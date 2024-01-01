namespace Game.Models;

public class CombatStateModel
{
    public int CurrentEnemyHp { get; set; }
    public HashSet<int> CleanedLocations { get; set; }
    
}