using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : Item, IConsumable
{
    public ActorController Owner;
    public int HealAmount;

    public override void Interact(ActorController interactingEntity)
    {
        Debug.Log("TEST");
        if (gameObject.activeInHierarchy)
        {
            entityManager = GameObject.FindObjectOfType<EntityManager>();
            Debug.Log("DREAMS");
            interactingEntity.Inventory.AddItem(this);
            Owner = interactingEntity;
            gameObject.SetActive(false);
            entityManager.interactables.Remove(this);
        }

    }

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
