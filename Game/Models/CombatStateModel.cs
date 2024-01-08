namespace Game.Models;

public class CombatStateModel
{
    public EnemyModel CurrentEnemy { get; set; }
    public bool IsCombatActive { get { 
        
            if(CurrentEnemy == null) return false;
            else if(CurrentEnemy.Hp > 0) return true;
            else return false;
        
    } }
    public HashSet<int> CleanedLocations { get; set; }
}