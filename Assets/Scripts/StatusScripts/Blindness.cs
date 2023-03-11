using RLDataTypes;

public class Blindness : Status
{
    public Blindness(ActorController actor, int turnsLeft) : base(StatusType.Blindness, actor, turnsLeft)
    {
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} was blinded!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} can see again!";
    }
}
