using RLDataTypes;

public class Muteness: Status
{

    public Muteness(ActorController actor, int turnsLeft) : base(StatusType.Muteness, actor, turnsLeft)
    {
        HUD_TEXT = "Muted";
    }

    public override bool IsMoveUsable(Move move)
    {
        return false;
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} was muted!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} is no longer muted!";
    }
}
