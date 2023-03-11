using RLDataTypes;

public class Poison : DamageOverTime
{
    private const int DAMAGE = 1;
    private const int DAMAGE_INTERVAL = 2;

    public Poison(ActorController actor, int turnsLeft) : base(StatusType.Poison, actor, turnsLeft, DAMAGE, DAMAGE_INTERVAL)
    {
        HUD_TEXT = "Poisoned";
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} was poisoned!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} was cured of poison!";
    }
}