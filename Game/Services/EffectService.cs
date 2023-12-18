namespace Game.Services;

public class EffectService
{
    public EffectService() { 
        
    }

    public void ApplyEffect(EffectModel e, PlayerModel p)
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

}