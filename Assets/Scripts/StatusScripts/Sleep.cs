using RLDataTypes;

public class Sleep : Status
{
    public Sleep(ActorController actor, int turnsLeft) : base(StatusType.Sleep, actor, turnsLeft)
    {
    }

    public override bool IsImmobilized()
    {
        return true;
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} fell asleep!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} woke up!";
    }
}