public class Punch : DealDamageFront
{
    public override void UseMove(ActorController user)
    {
        if (!user.Weapon)
        {
            base.UseMove(user);
        }
    }
    
    public override bool UsableByAI(ActorController user, ActorController target)
    {
        return !user.Weapon;
    }
}
