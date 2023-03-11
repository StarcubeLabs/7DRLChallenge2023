using RLDataTypes;

public class Petrify : Status
{
    public Petrify(ActorController actor, int turnsLeft) : base(StatusType.Petrify, actor, turnsLeft)
    {
        HUD_TEXT = "Petrified";
    }

    public override bool IsImmobilized()
    {
        return true;
    }

    public override void OnActorAttacked(ActorController attacker)
    {
        actor.CureStatus(this);
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} was petrified!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} is no longer petrified!";
    }
}