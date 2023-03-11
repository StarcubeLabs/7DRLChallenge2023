using RLDataTypes;

public class FlameShield : Status
{
    private const int BASE_DAMAGE = 2;
    
    public FlameShield(ActorController actor, int turnsLeft) : base(StatusType.FlameShield, actor, turnsLeft)
    {
        HUD_TEXT = "Flame Shielded";
    }

    public override void OnActorAttacked(ActorController attacker)
    {
        int damage = BASE_DAMAGE + DamageCalculator.GetTypeEffectivenessModifier(ElementType.Fire, attacker.GetEffectiveType());
        if (damage > 0)
        {
            attacker.Hurt(damage);
        }
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} set up a flame shield!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()}'s flame shield dissipated!";
    }
}
