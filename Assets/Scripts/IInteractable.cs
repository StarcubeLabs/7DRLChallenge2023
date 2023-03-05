using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// Allows you can interact with said Item such as pickup.
    /// </summary>
    void Interact(ActorController interactingEntity);

    public Vector3Int gridPosition
    {
        get;
        set;
    }
    
}
