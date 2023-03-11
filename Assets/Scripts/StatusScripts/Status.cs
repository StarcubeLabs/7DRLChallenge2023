using RLDataTypes;

public abstract class Status
{
    protected StatusType type;
    public StatusType Type { get { return type; } }
    
    protected int turnsLeft;
    protected ActorController actor;

    public Status(StatusType type, ActorController actor, int turnsLeft)
    {
        this.actor = actor;
        this.type = type;
        this.turnsLeft = turnsLeft;
    }

    public bool TickStatus()
    {
        if (turnsLeft-- <= 0)
        {
            return true;
        }
        OnTurnEnd();
        return false;
    }

    protected virtual void OnTurnEnd()
    {
    }

    public virtual void ModifyFacingDirection()
    {
    }

    public virtual bool IsImmobilized()
    {
        return false;
    }

    public virtual void OnActorAttacked(ActorController attacker)
    {
    }

    public virtual string GetStatusApplyMessage()
    {
        return null;
    }

    public virtual string GetStatusCureMessage()
    {
        return null;
    }
}
