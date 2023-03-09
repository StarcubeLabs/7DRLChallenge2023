using UnityEngine;

public class WalkAnimation : TurnAnimation
{
    private ActorController actor;
    private Animator actorAnimator;
    
    public WalkAnimation(ActorController actor, Animator actorAnimator)
    {
        this.actor = actor;
        this.actorAnimator = actorAnimator;
    }
    
    public override void StartAnimation()
    {
        actorAnimator?.SetTrigger("Walk");
    }

    public override bool UpdateAnimation()
    {
        return actor.UpdateVisualLocation();
    }

    public override bool CanRunAnimationsConcurrently(TurnAnimation anim)
    {
        return anim.GetType() == GetType();
    }
}
