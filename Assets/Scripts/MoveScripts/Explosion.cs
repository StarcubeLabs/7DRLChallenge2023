using UnityEngine;

public class Explosion : DealDamageAround
{
    
    public override void UseMove(ActorController user)
    {
        base.UseMove(user);
        if (user.hitPoints.x > 1)
        {
            user.Hurt(user.hitPoints.x - 1);
        }
    }
}
