using UnityEngine;

public class DealDamageAround : MoveData
{
    private Vector3Int[] AROUND_OFFSETS =
    {
        Vector3Int.up,
        new Vector3Int(1, 1),
        Vector3Int.right,
        new Vector3Int(1, -1),
        Vector3Int.down,
        new Vector3Int(-1, -1),
        Vector3Int.left,
        new Vector3Int(-1, 1)
    };
    
    public override void UseMove(ActorController user)
    {
        foreach (Vector3Int offset in AROUND_OFFSETS)
        {
            ActorController target = ServicesManager.EntityManager.getEntityInPosition(user.gridPosition + offset);
            if (target && !target.Dead)
            {
                DamageTarget(user, target);
            }
        }
    }

    public override bool InAIRange(ActorController user, ActorController target)
    {
        return ServicesManager.EntityManager.GetEntityInFront(user) == target;
    }
}
