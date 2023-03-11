using RLDataTypes;
using UnityEngine;

public class DealDamageFront : MoveData
{
    public override void UseMove(ActorController user)
    {
        ActorController target = ServicesManager.EntityManager.GetEntityInFront(user);
        if (target)
        {
            DamageTarget(user, target);

        }
    }
    
    public override bool UsableByAI(ActorController user, ActorController target)
    {
        if (Power == 0 && afflictionType != StatusType.None)
        {
            return !target.HasStatus(afflictionType);
        }
        return true;
    }

    public override bool InAIRange(ActorController user, ActorController target)
    {
        return ServicesManager.EntityManager.GetEntityInFront(user) == target;
    }

}
