namespace Game.Services;

public class EffectService
{
    public EffectService() { 
        
    }

    public static void ApplyEffect(EffectModel e, PlayerModel p)
    {
        switch (e.Type)
        {
            case EffectTypeModel.Health:
                p.Hp += e.EffectScale;
                break;
            case EffectTypeModel.Damage:
                p.Damage += e.EffectScale;
                if(p.Damage < 0) p.Damage = 0;
                break;
            case EffectTypeModel.Resistance:
                p.Resistance += e.EffectScale;
                if(p.Resistance < 0) p.Resistance = 0;
                break;
            case EffectTypeModel.Energy:
                p.Energy += e.EffectScale;
                if (p.Energy < 0) p.Energy = 0;
                break;
        }

    }

    public static void ApplyEffect(EffectModel e, EnemyModel p)
    {
        switch (e.Type)
        {
            case EffectTypeModel.Health:
                p.Hp += e.EffectScale;
                break;
            case EffectTypeModel.Damage:
                p.Damage += e.EffectScale;
                break;
        }

    }

    /// <summary>
    ///     Enemy deals his attacks to player. Uses EnemyModel.NumberOfAttacks, EnemyModel.Damage and PlayerModel.Resistence;
    /// </summary>
    /// <param name="e">Enemy model</param>
    /// <param name="p">Player model</param>
    public static void EnemyAttack(EnemyModel e, PlayerModel p)
    {
        if (p.CombatState.CurrentEnemy.Hp > 0)
        {
            for(int i = 0; i < e.NumberOfAttacks; i++)
            {
                int damage = -1 * CalculateDamage(e.Damage);
                damage = (int) (damage * (1.0 -  (double) p.Resistance / 200.0));
                if (damage > 0) damage = 0;

                ApplyEffect(new EffectModel { EffectScale = damage, Type = EffectTypeModel.Health }, p);
            }

        }
    }


    /// <summary>
    ///     
    /// </summary>
    /// <param name="d">Damage to calculate</param>
    /// <returns>Randomized damage as positive integer in boundry 0.8*d to 1.2*d</returns>
    public static int CalculateDamage(int d)
    {
        double randomizer = 0.8 + (Random.Shared.NextDouble() * 0.4);
        return (int)(randomizer * (double)d);
    }

}