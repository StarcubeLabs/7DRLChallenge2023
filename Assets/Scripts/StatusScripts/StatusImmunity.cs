using RLDataTypes;

public abstract class StatusImmunity : Status
{
    private StatusType immuneType;
    
    public StatusImmunity(StatusType type, ActorController actor, int turnsLeft, StatusType immuneType) : base(type, actor, turnsLeft)
    {
        this.immuneType = immuneType;
    }

    public override bool AllowStatus(StatusType statusType)
    {
        if (statusType == immuneType)
        {
            actor.CureStatus(this);
            return false;
        }

        return true;
    }
}
