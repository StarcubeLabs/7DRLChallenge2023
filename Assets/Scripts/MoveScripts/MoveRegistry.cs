using System.Collections.Generic;
using RLDataTypes;
using UnityEngine;

public class MoveRegistry : MonoBehaviour
{
    [SerializeField]
    private MoveData basicAttack;
    private Move basicAttackMove;
    public Move BasicAttack { get { return basicAttackMove; } }
    
    private StatusType[] HEALTH_DRAIN_STATUSES = { StatusType.Poison };
    private StatusType[] UNIQUE_STATUSES = { StatusType.Confusion };
    private StatusType[] MOVEMENT_STATUSES = { StatusType.Petrify, StatusType.Sleep, StatusType.Slow };
    private StatusType[] REGENERATION_STATUSES = { StatusType.Regeneration };

    private Dictionary<StatusType, StatusType> CONFLICTING_STATUSES = new Dictionary<StatusType, StatusType>();

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

        List<StatusType[]> statusGroups = new List<StatusType[]>();
        statusGroups.Add(HEALTH_DRAIN_STATUSES);
        statusGroups.Add(UNIQUE_STATUSES);
        statusGroups.Add(MOVEMENT_STATUSES);
        statusGroups.Add(REGENERATION_STATUSES);
        foreach (StatusType[] statusGroup in statusGroups)
        {
            foreach (StatusType status in statusGroup)
            {
                CONFLICTING_STATUSES.Add(status, statusGroup[0]);
            }
        }
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

    public Status CreateStatusFromType(StatusType status, ActorController actor, int turnCount)
    {
        switch (status)
        {
            case StatusType.Blindness: return new Blindness(actor, turnCount);
            case StatusType.Confusion: return new Confusion(actor, turnCount);
            case StatusType.Muteness: return new Muteness(actor, turnCount);
            case StatusType.Petrify: return new Petrify(actor, turnCount);
            case StatusType.Poison: return new Poison(actor, turnCount);
            case StatusType.Sleep: return new Sleep(actor, turnCount);
            case StatusType.Slow: return new Slow(actor, turnCount);
            default: return null;
        }
    }

    public bool DoStatusesConflict(StatusType status1, StatusType status2)
    {
        return status1 == status2 ||
               CONFLICTING_STATUSES.ContainsKey(status1) && CONFLICTING_STATUSES[status1] == CONFLICTING_STATUSES[status2];
    }
}

