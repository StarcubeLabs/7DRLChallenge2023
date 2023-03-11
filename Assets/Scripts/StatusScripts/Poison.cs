using RLDataTypes;

public class Poison : Status
{
    private const int DAMAGE = 1;
    private const int DAMAGE_INTERVAL = 2;
    private int turnDamage;
    
    public Poison(ActorController actor, int turnsLeft) : base(StatusType.Poison, actor, turnsLeft)
    {
        turnDamage = DAMAGE_INTERVAL;
    }
    
    protected override void OnTurnEnd()
    {
        if (--turnDamage <= 0)
        {
            actor.Hurt(DAMAGE);
            turnDamage = DAMAGE_INTERVAL;
        }
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