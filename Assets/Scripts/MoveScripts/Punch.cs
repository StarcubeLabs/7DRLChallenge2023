public class Punch : DealDamageFront
{
    public override void UseMove(ActorController user, EntityManager entityManager)
    {
        if (!user.Weapon)
        {
            base.UseMove(user, entityManager);
        }
    }
}
