using RLDataTypes;

public class EquippableItem : ItemData
{
    /// <summary>
    /// Modifies damage according to the user's equipped item.
    /// </summary>
    /// <param name="damage">Current damage value from the damage calculation.</param>
    /// <param name="user">User of the move dealing damage.</param>
    /// <param name="target">Target of the move dealing damage.</param>
    /// <returns>Modified damage value according to the user's equipped item.</returns>
    public virtual int ModifyDamageUser(int damage, ActorController user, ActorController target)
    {
        return damage;
    }
    
    /// <summary>
    /// Modifies damage according to the target's equipped item.
    /// </summary>
    /// <param name="damage">Current damage value from the damage calculation.</param>
    /// <param name="user">User of the move dealing damage.</param>
    /// <param name="target">Target of the move dealing damage.</param>
    /// <returns>Modified damage value according to the target's equipped item.</returns>
    public virtual int ModifyDamageTarget(int damage, ActorController user, ActorController target)
    {
        return damage;
    }
    
    /// <summary>
    /// Modifies the target's defensive type according to the target's equipped item.
    /// </summary>
    /// <param name="elementType">Target's current defensive type</param>
    /// <param name="target">Target of the move dealing damage.</param>
    /// <returns>Modified target defensive type if needed.</returns>
    public virtual ElementType ModifyTypeTarget(ElementType elementType, ActorController target)
    {
        return elementType;
    }

    /// <summary>
    /// Called after a move successfully damages a target.
    /// </summary>
    /// <param name="user">The user of the move.</param>
    /// <param name="target">The target of the move.</param>
    public virtual void OnDamageDealt(ActorController user, ActorController target)
    {
    }

    public virtual void OnActorAttacked(ActorController attacker, int damage)
    {
    }

    /// <summary>
    /// Checks if a status can be applied to the target.
    /// </summary>
    /// <param name="target">The target of the status.</param>
    /// <param name="statusType">The status being applied.</param>
    /// <returns>True if the status can be applied.</returns>
    public virtual bool AllowStatus(ActorController target, StatusType statusType)
    {
        return true;
    }

    public virtual int ModifyHungerDrainRate(int hungerDrainRate)
    {
        return hungerDrainRate;
    }
}
