using UnityEngine;

public class ForceLocationAnimation : UpdateLocationAnimation
{
    private const float ANIMATION_TIME = 2;
    
    public ForceLocationAnimation(ActorController actor, Animator actorAnimator) : base(actor, actorAnimator, ANIMATION_TIME)
    {
    }
    
    public override void StartAnimation()
    {
        actorAnimator?.SetTrigger("Hurt");
    }
}
