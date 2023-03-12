using RLDataTypes;
using UnityEngine;
using System;

[System.Serializable]
public abstract class MoveData : MonoBehaviour
{
    [SerializeField]
    protected string moveName;
    public string MoveName { get { return moveName; } }

    [SerializeField]
    protected string moveDescription;
    public string MoveDescription { get { return moveDescription; } }
    
    protected ElementType element = ElementType.Neutral;
    public ElementType Element = ElementType.Neutral;

    [SerializeField]
    protected int power;
    public int Power { get { return power; } }

    [SerializeField]
    protected int maxPP;
    public int MaxPP { get { return maxPP; } }

    [SerializeField]
    protected StatusType afflictionType = StatusType.None;

    [SerializeField]
    [Range(0f, 1f)]
    protected float afflictionChance = 0f;

    [SerializeField]
    protected int afflictionTurnCount;


    public ParticleSystem launchVFX;

    public ParticleSystem hitVFX;

    public abstract void UseMove(ActorController user);

    /// <summary>
    /// Modifies the move's element from its base element if needed.
    /// </summary>
    /// <param name="user">The user of the move.</param>
    /// <returns></returns>
    public virtual ElementType GetModifiedElement(ActorController user)
    {
        return element;
    }

    protected void DamageTarget(ActorController user, ActorController target)
    {
        int damage = 0;
        if (Power > 0)
        {
            damage = user.DamageTarget(this, target);
        }
        if (afflictionType != StatusType.None && (Power == 0 || damage > 0) && UnityEngine.Random.value <= afflictionChance)
        {
            target.ApplyStatus(afflictionType, afflictionTurnCount);
        }
    }


    /// <summary>
    /// Returns true if the AI should consider the move in range of the target.
    /// </summary>
    /// <param name="user">The user of the move.</param>
    /// <param name="target">The intended target of the move.</param>
    /// <returns>True if the move is in range for the purposes of the AI.</returns>
    public virtual bool InAIRange(ActorController user, ActorController target)
    {
        return false;
    }

    /// <summary>
    /// Returns true if the move can be used by the AI in this turn. This handles all non-range condition, such as applying a duplicate status.
    /// </summary>
    /// <param name="user">The user of the move.</param>
    /// <param name="target">The intended target of the move.</param>
    /// <returns>True if the move can be used by the AI in this turn.</returns>
    public virtual bool UsableByAI(ActorController user, ActorController target)
    {
        return true;
    }

    public virtual int GetMovePower()
    {
        return Power;
    }

    public virtual void TriggerUserVFX(ActorController user)
    {
        if (launchVFX)
        {
            Instantiate(launchVFX, user.transform, false);
        }
    }

    public virtual void TriggerTargetVFX(ActorController target)
    {
        if (hitVFX)
        {
            //Hit VFX
            Instantiate(hitVFX, target.transform, false);
        }
    }
}

