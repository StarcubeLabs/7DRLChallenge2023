using RLDataTypes;

public class Slow : Status
{

    private bool canMove;
    
    public Slow(ActorController actor, int turnsLeft) : base(StatusType.Slow, actor, turnsLeft)
    {
        HUD_TEXT = "Slowed";
    }

    protected override void OnTurnEnd()
    {
        canMove = !canMove;
    }

    public override bool IsImmobilized()
    {
        return canMove;
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} was slowed!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} returned to normal speed!";
    }
}
