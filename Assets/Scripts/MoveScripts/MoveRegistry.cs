using System.Collections.Generic;
using RLDataTypes;
using UnityEngine;

public class MoveRegistry : MonoBehaviour
{
    [SerializeField]
    private MoveData basicAttack;
    private Move basicAttackMove;
    public Move BasicAttack { get { return basicAttackMove; } }
    
    private StatusType[] HEALTH_DRAIN_STATUSES = { StatusType.Burn, StatusType.Poison };
    private StatusType[] UNIQUE_STATUSES = { StatusType.Confusion, StatusType.Frozen };
    private StatusType[] MOVEMENT_STATUSES = { StatusType.Petrify, StatusType.SeismicShock, StatusType.Sleep, StatusType.Slow, StatusType.Stun };
    private StatusType[] SHIELD_STATUSES = { StatusType.FlameShield };
    private StatusType[] REGENERATION_STATUSES = { StatusType.Regeneration };
    private StatusType[] IMMUNITY_STATUSES = { StatusType.PetrifyImmunity, StatusType.SleepImmunity };

    private Dictionary<StatusType, StatusType> CONFLICTING_STATUSES = new Dictionary<StatusType, StatusType>();

    [SerializeField]
    private Sprite atkUpSprite;
    [SerializeField]
    private Sprite blindnessSprite;
    [SerializeField]
    private Sprite burnSprite;
    [SerializeField]
    private Sprite confusionSprite;
    [SerializeField]
    private Sprite defUpSprite;
    [SerializeField]
    private Sprite flameShieldSprite;
    [SerializeField]
    private Sprite frozenSprite;
    [SerializeField]
    private Sprite mutenessSprite;
    [SerializeField]
    private Sprite petrifiedSprite;
    [SerializeField]
    private Sprite petrifyImmunitySprite;
    [SerializeField]
    private Sprite poisonSprite;
    [SerializeField]
    private Sprite regenerationSprite;
    [SerializeField]
    private Sprite seismicShockSprite;
    [SerializeField]
    private Sprite sleepSprite;
    [SerializeField]
    private Sprite sleepImmunitySprite;
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
        statusGroups.Add(SHIELD_STATUSES);
        statusGroups.Add(REGENERATION_STATUSES);
        statusGroups.Add(IMMUNITY_STATUSES);
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
            case StatusType.AttackUp: return atkUpSprite;
            case StatusType.Blindness: return blindnessSprite;
            case StatusType.Burn: return burnSprite;
            case StatusType.Confusion: return confusionSprite;
            case StatusType.DefenseUp: return defUpSprite;
            case StatusType.FlameShield: return flameShieldSprite;
            case StatusType.Frozen: return frozenSprite;
            case StatusType.Muteness: return mutenessSprite;
            case StatusType.Petrify: return petrifiedSprite;
            case StatusType.PetrifyImmunity: return petrifyImmunitySprite;
            case StatusType.Poison: return poisonSprite;
            case StatusType.Regeneration: return regenerationSprite;
            case StatusType.SeismicShock: return seismicShockSprite;
            case StatusType.Sleep: return sleepSprite;
            case StatusType.SleepImmunity: return sleepImmunitySprite;
            case StatusType.Slow: return slowSprite;
            case StatusType.Stun: return stunSprite;
            default: return null;
        }
    }

    public Status CreateStatusFromType(StatusType status, ActorController actor, int turnCount)
    {
        switch (status)
        {
            case StatusType.AttackUp: return new AttackUp(actor, turnCount);
            case StatusType.Blindness: return new Blindness(actor, turnCount);
            case StatusType.Burn: return new Burn(actor, turnCount);
            case StatusType.Confusion: return new Confusion(actor, turnCount);
            case StatusType.DefenseUp: return new DefenseUp(actor, turnCount);
            case StatusType.FlameShield: return new FlameShield(actor, turnCount);
            case StatusType.Frozen: return new Frozen(actor, turnCount);
            case StatusType.Muteness: return new Muteness(actor, turnCount);
            case StatusType.Petrify: return new Petrify(actor, turnCount);
            case StatusType.PetrifyImmunity: return new PetrifyImmunity(actor, turnCount);
            case StatusType.Poison: return new Poison(actor, turnCount);
            case StatusType.Regeneration: return new Regeneration(actor, turnCount);
            case StatusType.SeismicShock: return new SeismicShock(actor, turnCount);
            case StatusType.Sleep: return new Sleep(actor, turnCount);
            case StatusType.SleepImmunity: return new SleepImmunity(actor, turnCount);
            case StatusType.Slow: return new Slow(actor, turnCount);
            case StatusType.Stun: return new Stun(actor, turnCount);
            default: return null;
        }
    }

    public bool DoStatusesConflict(StatusType status1, StatusType status2)
    {
        return status1 == status2 ||
               CONFLICTING_STATUSES.ContainsKey(status1) && CONFLICTING_STATUSES[status1] == CONFLICTING_STATUSES[status2];
    }
}

