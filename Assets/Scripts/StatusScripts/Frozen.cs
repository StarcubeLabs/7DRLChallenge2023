using RLDataTypes;

public class Frozen : Status
{
    public Frozen(ActorController actor, int turnsLeft) : base(StatusType.Frozen, actor, turnsLeft)
    {
    }

    public override bool IsImmobilized()
    {
        return true;
    }
    
    public override int ModifyDamageTarget(int damage, ActorController user)
    {
        return 0;
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} was frozen!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} thawed out!";
    }
}
