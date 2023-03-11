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

    public Item ItemToDrop;
    public int NumberOfItemsToDrop;
    
    public void InitializeInventory(int numberOfSlots)
    {
        NumberOfSlots = numberOfSlots;
        Items = new Item[numberOfSlots];
    }
    
    public bool AddItem(Item itemToAdd)
    {
        if (itemToAdd.CurrentNumberOfStacks == 0)
        {
            itemToAdd.CurrentNumberOfStacks = 1;
            Debug.LogWarning("Are you sure you want to define your stack as 0.");
        }
        for (int index = 0; index < Items.Length; ++index)
        {
            if (itemToAdd.CanBeStacked)
            {
                foreach (var item in Items.Where(item => item!=null && item.CanBeStacked))
                {
                    if (item.CurrentNumberOfStacks < item.MaximumNumberOfStacks)
                    {
                        item.CurrentNumberOfStacks += itemToAdd.CurrentNumberOfStacks;
                        return true;
                    }
                }
                if(Items[index]==null)
                {
                    Items[index] = itemToAdd;
                    return true;
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
        if (itemToDrop == null)
        {
            return false;
        }
        entityManager = GameObject.FindObjectOfType<EntityManager>();
        if (entityManager.isInteractableInPosition(positionToDrop))
        {
            return false;
        }
        if (HasItem(itemToDrop))
        {
            if (itemToDrop.CanBeStacked)
            {
                if (numberToDrop > 0)
                {
                    var itemIndex = Array.IndexOf(Items, itemToDrop);
                    itemToDrop.CurrentNumberOfStacks -= numberToDrop;
                    if (itemToDrop.CurrentNumberOfStacks == 0)
                    {
                        Items[itemIndex] = null;
                    }

                    Item itemStack = GameObject.Instantiate(itemToDrop);

                    itemStack.gridPosition = positionToDrop;
                    itemStack.CurrentNumberOfStacks = numberToDrop;
                    itemStack.SnapToPosition(positionToDrop);
                    itemStack.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    itemStack.gameObject.SetActive(true);
                    entityManager.interactables.Add(itemStack);
                    return true;
                }
            }
            else
            {
                var itemIndex = Array.IndexOf(Items, itemToDrop);
                itemToDrop.gridPosition = positionToDrop;
                itemToDrop.SnapToPosition(positionToDrop);
                itemToDrop.gameObject.SetActive(true);
                Items[itemIndex] = null;
                entityManager.interactables.Add(itemToDrop);
                return true;
            }
        }
        return false;
    }

    public bool PreventDeath(ActorController actor)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Item item = Items[i];
            if (item && item.ItemData.PreventDeath(actor, item))
            {
                GameObject.Destroy(Items[i].gameObject);
                Items[i] = null;
                return true;
            }
        }

        return false;
    }
}
