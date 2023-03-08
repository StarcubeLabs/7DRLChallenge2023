using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : ItemData
{
    public int power;

    public override bool OnConsume(ActorController consumer)
    {
        consumer.EquipWeapon(this);
        return false;
    }
}
