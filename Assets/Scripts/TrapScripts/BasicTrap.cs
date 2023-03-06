using RLDataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicTrap : ScriptableObject
{

    /// <summary>
    /// The status type that this trap applies when stepped on.
    /// </summary>
    public StatusType trapStatusType = StatusType.None;

    /// <summary>
    /// The amount of turns the affliction should last when applied to an actor.
    /// </summary>
    public int statusTurnCount = 2;

    /// <summary>
    /// How much of a chance the trap has of afflicting when activated.
    /// </summary>
    [Range(0f, 1f)]
    public float successChance = 0.25f;

    /// <summary>
    /// Whether or not the trap is currently visisble/revealed.
    /// </summary>
    public bool isVisible = false;

    /// <summary>
    /// Applies this trap's effect to the target actor.
    /// </summary>
    /// <param name="actor"></param>

    public GameObject trapVisual;

    public Sprite trapVisual_Temp;
    
    public virtual void ActivateTrap(ActorController actor)
    {
        actor.ApplyStatus(trapStatusType, statusTurnCount);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        trapVisual.SetActive(isVisible);
    }
}
