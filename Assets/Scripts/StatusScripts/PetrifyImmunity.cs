using RLDataTypes;

public class PetrifyImmunity : StatusImmunity
{
    public PetrifyImmunity(ActorController actor, int turnsLeft) : base(StatusType.PetrifyImmunity, actor, turnsLeft, StatusType.Petrify)
    {
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} is immune to petrification!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} is no longer immune to petrification!";
    }
}