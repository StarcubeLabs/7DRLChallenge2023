using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public string ItemName;
    public string ItemDescription;
    public Sprite ItemIcon;
    public long ItemValue;
    public bool IsStackable;
    public int MaximumStack;

    /// <summary>a
    /// Handles what happens when you consume said item.
    /// </summary>
    /// <param name="consumer">The actor consuming the item.</param>
    /// <returns>True if the item should be consumed from the inventory.</returns>
    public virtual bool OnConsume(ActorController consumer)
    {
        return false;
    }
}
