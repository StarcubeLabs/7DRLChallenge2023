using RLDataTypes;

public class DefenseUp : Status
{
    private const int DAMAGE_DECREASE = 1;
    
    public DefenseUp(ActorController actor, int turnsLeft) : base(StatusType.DefenseUp, actor, turnsLeft)
    {
    }

    public override int ModifyDamageTarget(int damage, ActorController user)
    {
        return damage - DAMAGE_DECREASE;
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()}'s defense rose!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()}'s defense returned to normal!";
    }
}
