using RLDataTypes;

public class Regeneration : Status
{
    private const int HEAL_PER_TURN = 1;
    
    public Regeneration(ActorController actor, int turnsLeft) : base(StatusType.Regeneration, actor, turnsLeft)
    {
    }

    protected override void OnTurnEnd()
    {
        actor.HealAmount(HEAL_PER_TURN);
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} is regenerating health!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} stopped regenerating health!";
    }
}
