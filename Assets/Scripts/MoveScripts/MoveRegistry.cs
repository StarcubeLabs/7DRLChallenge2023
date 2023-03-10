using RLDataTypes;
using UnityEngine;

public class MoveRegistry : MonoBehaviour
{
    [SerializeField]
    private MoveData basicAttack;
    private Move basicAttackMove;
    public Move BasicAttack { get { return basicAttackMove; } }

    [SerializeField]
    private Sprite atkDownSprite;
    [SerializeField]
    private Sprite atkUpSprite;
    [SerializeField]
    private Sprite burnSprite;
    [SerializeField]
    private Sprite defDownSprite;
    [SerializeField]
    private Sprite defUpSprite;
    [SerializeField]
    private Sprite petrifiedSprite;
    [SerializeField]
    private Sprite poisonSprite;
    [SerializeField]
    private Sprite seismicShockSprite;
    [SerializeField]
    private Sprite sleepSprite;
    [SerializeField]
    private Sprite slowSprite;
    [SerializeField]
    private Sprite stunSprite;

    public void Start()
    {
        basicAttackMove = Move.InitiateFromMoveData(basicAttack);
    }

    public Sprite GetStatusSprite(StatusType status)
    {
        switch (status)
        {
            case StatusType.Petrify: return petrifiedSprite;
            case StatusType.Poison: return poisonSprite;
            case StatusType.Sleep: return sleepSprite;
            case StatusType.Slow: return slowSprite;
            default: return null;
        }
    }
}

