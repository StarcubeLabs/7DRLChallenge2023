using RLDataTypes;

public class Burn : DamageOverTime
{
    private const int DAMAGE = 1;
    private const int DAMAGE_INTERVAL = 1;
    
    public Burn(ActorController actor, int turnsLeft) : base(StatusType.Burn, actor, turnsLeft, DAMAGE, DAMAGE_INTERVAL)
    {
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} was burned!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} is no longer burned!";
    }
}
