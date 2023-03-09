public class DealDamageFront : MoveData
{
    public override void UseMove(ActorController user)
    {
        ActorController target = ServicesManager.EntityManager.getEntityInPosition(user.GetPositionInFront());
        if (target)
        {
            user.DamageTarget(this, target);
        }
    }
}
