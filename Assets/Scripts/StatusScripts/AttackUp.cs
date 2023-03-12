using RLDataTypes;

public class AttackUp : Status
{
    private const int DAMAGE_INCREASE = 1;
    
    public AttackUp(ActorController actor, int turnsLeft) : base(StatusType.AttackUp, actor, turnsLeft)
    {
    }

    public override int ModifyDamageUser(int damage, ActorController target)
    {
        return damage + DAMAGE_INCREASE;
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()}'s attack rose!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()}'s attack returned to normal!";
    }
}
