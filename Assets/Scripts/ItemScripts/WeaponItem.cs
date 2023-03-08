using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : ItemData
{
    public int power;

    public override bool OnConsume(ActorController consumer, Item item)
    {
        consumer.EquipWeapon(item);
        return false;
    }

    public override void OnDrop(ActorController owner, Item item)
    {
        owner.UnequipWeapon(item);
    }
}
