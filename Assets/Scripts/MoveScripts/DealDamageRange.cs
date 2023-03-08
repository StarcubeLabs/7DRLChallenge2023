using UnityEngine;

public class DealDamageRange : MoveData
{
    [SerializeField]
    private int range;
    
    public override void UseMove(ActorController user, EntityManager entityManager)
    {
        for (int i = 1; i <= range; i++)
        {
            ActorController entityToAttack = entityManager.getEntityInPosition(user.GetPositionInFront(i));
            if (entityToAttack)
            {
                entityToAttack.Hurt(user.AttackPower + power);
                break;
            }
        }
    }
}
