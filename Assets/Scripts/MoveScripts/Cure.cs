using UnityEngine;

public class Cure : MoveData
{
    [SerializeField]
    private int healAmount;
    
    public override void UseMove(ActorController user)
    {
        user.HealAmount(healAmount);
    }

    public override void TriggerVFX(ActorController user)
    {
        if (launchVFX != null)
        {
            GameObject.Instantiate<ParticleSystem>(launchVFX, user.transform, false);
        }
    }
}
