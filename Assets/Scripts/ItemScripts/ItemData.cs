using System;
using UnityEngine;

[Serializable]
public class ItemData : MonoBehaviour
{
    public string ItemName;
    public string ItemDescription;
    public Sprite ItemIcon;
    public GameObject ItemObject;
    public bool IsStackable;
    public int MaximumStack = 1;

    /// <summary>a
    /// Handles what happens when you consume said item.
    /// </summary>
    /// <param name="consumer">The actor consuming the item.</param>
    /// <param name="item">Item entity that the item data is attached to.</param>
    /// <returns>True if the item should be consumed from the inventory.</returns>
    public virtual bool OnConsume(ActorController consumer, Item item)
    {
        return false;
    }

    /// <summary>
    /// Triggers when the player drops the item.
    /// </summary>
    /// <param name="owner">Actor who dropped the item.</param>
    /// <param name="item">Item instance that was dropped.</param>
    public virtual void OnDrop(ActorController owner, Item item)
    {
    }

    public virtual bool PreventDeath(ActorController owner, Item item)
    {
        return false;
    }
}
