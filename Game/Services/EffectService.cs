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
                break;
            case EffectTypeModel.Resistance:
                p.Resistance += e.EffectScale;
                break;
            case EffectTypeModel.Energy:
                p.Energy += e.EffectScale;
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
    ///     Enemy deals his attacks to player. Uses EnemyModel.NumberOfAttacks
    /// </summary>
    /// <param name="e"></param>
    /// <param name="p"></param>
    public static void EnemyAttack(EnemyModel e, PlayerModel p)
    {
        if (p.CombatState.CurrentEnemy.Hp > 0)
        {
            for(int i = 0; i < e.NumberOfAttacks; i++)
            {
                int damage = CalculateDamage(e.Damage);
                ApplyEffect(new EffectModel { EffectScale = -1 * damage, Type = EffectTypeModel.Health }, p);
            }

        }
    }

    public static int CalculateDamage(int d)
    {
        switch (Random.Shared.Next(0, 10))
        {
            // weak attack
            case 0:
                return d * 2;
            // strong attack
            case 1:
                return d / 2;
            default:
                return d;
        }
    }

}