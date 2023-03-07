using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : Item, IConsumable
{
    public int HealAmount;

    public void OnConsume()
    {
        int itemIndex = Array.IndexOf(Owner.Inventory.Items, this);
        Owner.Inventory.Items[itemIndex] = null;
        Owner.HealAmount(HealAmount);
        if (CanBeStacked)
        {
            --CurrentNumberOfStacks;
            if (CurrentNumberOfStacks == 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
