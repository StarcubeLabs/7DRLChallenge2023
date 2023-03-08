using UnityEngine;

public class DealDamage : MoveData
{
    public override void UseMove(ActorController user, EntityManager entityManager)
    {
        ActorController entityToAttack = entityManager.getEntityInPosition(user.GetPositionInFront());
        if (entityToAttack)
        {
            entityToAttack.Hurt(user.AttackPower + power);
        }
    }
}
