using RLDataTypes;

public class Stun : Status
{
    public Stun(ActorController actor, int turnsLeft) : base(StatusType.Stun, actor, turnsLeft)
    {
        HUD_TEXT = "Stunned";
    }

    public override bool IsImmobilized()
    {
        return true;
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} was stunned!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} is no longer stunned!";
    }
}
