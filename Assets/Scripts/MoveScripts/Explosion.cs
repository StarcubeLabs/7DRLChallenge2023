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

    public override void TriggerVFX(ActorController user)
    {
        if (launchVFX != null)
        {
            GameObject.Instantiate<ParticleSystem>(launchVFX, user.transform, false);
        }
    }
}
