using UnityEngine;

public abstract class MoveData : MonoBehaviour
{
    [SerializeField]
    protected string moveName;
    public string MoveName { get { return moveName; } }

    [SerializeField]
    protected string moveDescription;
    public string MoveDescription { get { return moveDescription; } }

    [SerializeField]
    protected int power;
    public int Power { get { return power; } }

    [SerializeField]
    protected int maxPP;
    public int MaxPP { get { return maxPP; } }

    public abstract void UseMove(ActorController user, EntityManager entityManager);
}

