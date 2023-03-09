using UnityEngine;

public class DealDamageRange : MoveData
{
    [SerializeField]
    private int range;
    
    public override void UseMove(ActorController user)
    {
        for (int i = 1; i <= range; i++)
        {
            Vector3Int attackPosition = user.GetPositionInFront(i);
            if (!ServicesManager.LevelManager.GetActiveLevel().IsCellWalkable(attackPosition))
            {
                break;
            }
            
            ActorController target = ServicesManager.EntityManager.getEntityInPosition(attackPosition);
            if (target && !target.Dead)
            {
                user.DamageTarget(this, target);
                break;
            }
        }
    }
}
