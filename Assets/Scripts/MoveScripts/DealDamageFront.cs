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

    public override bool InAIRange(ActorController user, ActorController target)
    {
        return ServicesManager.EntityManager.GetEntityInFront(user) == target;
    }
}
