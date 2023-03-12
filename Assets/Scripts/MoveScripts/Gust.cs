using UnityEngine;

public class Gust : MoveData
{
    [SerializeField]
    private int maxKnockbackTiles;
    
    public override void UseMove(ActorController user)
    {
        ActorController target = ServicesManager.EntityManager.GetEntityInFront(user);
        if (target)
        {
            DamageTarget(user, target);
            if (target.Dead)
            {
                return;
            }

            bool collided = false;
            for (int i = 2; i < maxKnockbackTiles; i++)
            {
                Vector3Int aheadPosition = user.GetPositionInFront(i);
                Vector3Int knockbackPosition = user.GetPositionInFront(i - 1);

                ActorController knockbackCollision = ServicesManager.EntityManager.getEntityInPosition(aheadPosition);
                if (knockbackCollision || !ServicesManager.LevelManager.GetActiveLevel().IsCellWalkable(aheadPosition))
                {
                    target.ForceLocation(knockbackPosition);
                    
                    target.ApplyStatus(afflictionType, afflictionTurnCount);
                    if (knockbackCollision)
                    {
                        knockbackCollision.ApplyStatus(afflictionType, afflictionTurnCount);
                    }

                    collided = true;

                    break;
                }
            }

            if (!collided)
            {
                target.ForceLocation(user.GetPositionInFront(maxKnockbackTiles));
            }
        }
    }

    public override bool InAIRange(ActorController user, ActorController target)
    {
        return ServicesManager.EntityManager.GetEntityInFront(user) == target;
    }
}
