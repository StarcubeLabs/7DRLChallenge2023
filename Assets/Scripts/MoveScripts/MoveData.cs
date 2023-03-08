using RLDataTypes;
using UnityEngine;

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

    public abstract void UseMove(ActorController user, EntityManager entityManager);

    /// <summary>
    /// Modifies the move's element from its base element if needed.
    /// </summary>
    /// <param name="user">The user of the move.</param>
    /// <returns></returns>
    public virtual ElementType GetModifiedElement(ActorController user)
    {
        return element;
    }
}

