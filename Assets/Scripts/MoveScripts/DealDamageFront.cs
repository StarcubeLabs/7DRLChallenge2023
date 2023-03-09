public class DealDamageFront : MoveData
{
    public override void UseMove(ActorController user, EntityManager entityManager)
    {
        ActorController target = entityManager.getEntityInPosition(user.GetPositionInFront());
        if (target)
        {
            user.DamageTarget(this, target);
        }
    }
}
