using UnityEngine;
using RLDataTypes;

public class BaseWeapon : EquippableItem
{
    /// <summary>
    /// The weapon's Element Type.
    /// </summary>
    public ElementType weaponElement = ElementType.Neutral;

    /// <summary>
    /// The base number that gets added to attack strength when the weapon is used.
    /// </summary>
    public int WeaponStrength = 1;

    /// <summary>
    /// The Status Type that the weapon has a chance of afflicting.
    /// </summary>
    public StatusType AfflictionType = StatusType.None;

    /// <summary>
    /// Percentage Chance of Target being afflicted with the selected StatusType, from 0 to 1 (or 0% to 100%).
    /// </summary>
    [Range(0f, 1f)]
    public float AfflictionChance = 0f;

    public int AfflictionTurnCount;

    public override bool OnConsume(ActorController consumer, Item item)
    {
        consumer.EquipWeapon(item);
        return false;
    }

    public override void OnDrop(ActorController owner, Item item)
    {
        owner.UnequipWeapon(item);
    }
    
    public override int ModifyDamageUser(int damage, ActorController user, ActorController target)
    {
        return damage + WeaponStrength;
    }

    public override void OnDamageDealt(ActorController user, ActorController target)
    {
        if (AfflictionType != StatusType.None && Random.value <= AfflictionChance)
        {
            target.ApplyStatus(AfflictionType, AfflictionTurnCount);
        }
    }
}
