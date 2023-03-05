using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    public EntityManager entityManager;
    public Item[] Items;
    public int NumberOfSlots;
    public void InitializeInventory(int numberOfSlots)
    {
        NumberOfSlots = numberOfSlots;
        Items = new Item[numberOfSlots];
    }
    
    public bool AddItem(Item itemToAdd)
    {
        for (int index = 0; index < Items.Length; ++index)
        {
            if (itemToAdd.CanBeStacked)
            {
                foreach (var item in Items.Where(item => item.CanBeStacked))
                {
                    if (item.CurrentNumberOfStacks < item.MaximumNumberOfStacks)
                    {
                        ++item.CurrentNumberOfStacks;
                        return true;
                    }
                }
            }
            else
            {
                if(Items[index]==null)
                {
                    Items[index] = itemToAdd;
                    return true;
                }
            }
        }
        return false;
    }

    bool HasItem(Item itemToCheck)
    {
        for (int index = 0; index < Items.Length; ++index)
        {
            if (Items[index] == itemToCheck)
            {
                return true;
            }
        }

        return false;
    }
    
    public bool DropItem(Item itemToDrop,  Vector3Int positionToDrop, int numberToDrop=0)
    {
        entityManager = GameObject.FindObjectOfType<EntityManager>();
        if (entityManager.isInteractableInPosition(positionToDrop))
        {
            return false;
        }
        if (HasItem(itemToDrop))
        {
            if (itemToDrop.CanBeStacked)
            {
                itemToDrop.CurrentNumberOfStacks -= numberToDrop;
                Item itemStack = GameObject.Instantiate(itemToDrop);
                itemStack.gridPosition = positionToDrop;
                itemStack.CurrentNumberOfStacks = numberToDrop;
                itemStack.SnapToPosition(positionToDrop);
                entityManager.interactables.Add(itemStack);
                return true;
            }

            int itemIndex = Array.IndexOf(Items, itemToDrop);
            itemToDrop.gridPosition = positionToDrop;
            itemToDrop.SnapToPosition(positionToDrop);
            itemToDrop.gameObject.SetActive(true);
            Items[itemIndex] = null;
            entityManager.interactables.Add(itemToDrop);
            return true;
        }
        return false;
    }
}
