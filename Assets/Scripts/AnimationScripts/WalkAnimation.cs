using UnityEngine;

public class WalkAnimation : UpdateLocationAnimation
{
    private const float ANIMATION_TIME = 0.25f;
    
    public WalkAnimation(ActorController actor, Animator actorAnimator) : base(actor, actorAnimator, ANIMATION_TIME)
    {
    }
    
    public override void StartAnimation()
    {
        actorAnimator?.SetTrigger("Walk");
    }

    public override bool CanRunAnimationsConcurrently(TurnAnimation anim)
    {
        return anim.GetType() == GetType();
    }
}
