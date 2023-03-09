public class HiltStrike : DealDamageFront
{
    public override void UseMove(ActorController user)
    {
        if (user.Weapon)
        {
            base.UseMove(user);
        }
    }
}
