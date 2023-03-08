using UnityEngine;

public class DealDamageRange : MoveData
{
    [SerializeField]
    private int range;
    
    public override void UseMove(ActorController user, EntityManager entityManager)
    {
        for (int i = 1; i <= range; i++)
        {
            ActorController target = entityManager.getEntityInPosition(user.GetPositionInFront(i));
            if (target)
            {
                target.Hurt(DamageCalculator.CalculateDamage(this, user, target));
                break;
            }
        }
    }
}
