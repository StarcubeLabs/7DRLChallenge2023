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
}
