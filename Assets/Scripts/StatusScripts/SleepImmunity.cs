using RLDataTypes;

public class SleepImmunity : StatusImmunity
{
    public SleepImmunity(ActorController actor, int turnsLeft) : base(StatusType.SleepImmunity, actor, turnsLeft, StatusType.Sleep)
    {
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} is now sleepless!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} is no longer sleepless!";
    }
}
