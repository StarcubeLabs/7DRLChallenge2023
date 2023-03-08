using UnityEngine;
using RLDataTypes;

public class BaseArmor : EquippableItem
{
    /// <summary>
    /// The element that determines what attacks against the user are Not Effective and Super Effective
    /// </summary>
    public ElementType armorElement = ElementType.Neutral;

    /// <summary>
    /// The number that reduces how much damage is deal to the user.
    /// </summary>
    public int ArmorStrength = 1;

    /// <summary>
    /// The Status Type that this armor blocks. Choosing (All) makes it so it checks for all types.
    /// </summary>
    public StatusType StatusBlockType = StatusType.None;

    /// <summary>
    /// Percentage Chance of blocking the targeted Status Type, as a range from 0 to 1 (0% to 100%)l
    /// </summary>
    [Range(0f, 1f)]
    public float ProtectionChance = 0f;

    public override bool OnConsume(ActorController consumer, Item item)
    {
        consumer.EquipArmor(item);
        return false;
    }

    public override void OnDrop(ActorController owner, Item item)
    {
        owner.UnequipArmor(item);
    }
    
    public override int ModifyDamageTarget(int damage, ActorController user, ActorController target)
    {
        return damage - ArmorStrength;
    }
}
