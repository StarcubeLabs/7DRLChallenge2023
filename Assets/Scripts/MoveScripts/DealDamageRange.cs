using UnityEngine;

public class DealDamageRange : MoveData
{
    [SerializeField]
    private int range;
    
    public override void UseMove(ActorController user)
    {
        ActorController target = GetEntityInRange(user);
        if (target)
        {
            DamageTarget(user, target);
        }
    }

    public override bool InAIRange(ActorController user, ActorController target)
    {
        return GetEntityInRange(user) == target;
    }

    private ActorController GetEntityInRange(ActorController user)
    {
        for (int i = 1; i <= range; i++)
        {
            Vector3Int attackPosition = user.GetPositionInFront(i);
            if (!ServicesManager.LevelManager.GetActiveLevel().IsCellWalkable(attackPosition))
            {
                return null;
            }
            
            ActorController target = ServicesManager.EntityManager.getEntityInPosition(attackPosition);
            if (target && !target.Dead)
            {
                return target;
            }
        }

        return null;
    }
}
