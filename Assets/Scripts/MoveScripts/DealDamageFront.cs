public class DealDamageFront : MoveData
{
    public override void UseMove(ActorController user)
    {
        ActorController target = GetEntityInFront(user);
        if (target)
        {
            DamageTarget(user, target);
        }
    }

    public override bool InAIRange(ActorController user, ActorController target)
    {
        return GetEntityInFront(user) == target;
    }

    private ActorController GetEntityInFront(ActorController user)
    {
        return ServicesManager.EntityManager.getEntityInPosition(user.GetPositionInFront());;
    }
}
