using UnityEngine;

public class AnimatorAnimation : TurnAnimation
{
    private Animator actorAnimator;
    private string animationName;
    
    public AnimatorAnimation(Animator actorAnimator, string animationName)
    {
        this.actorAnimator = actorAnimator;
        this.animationName = animationName;
    }
    
    public override void StartAnimation()
    {
        actorAnimator?.SetTrigger(animationName);
    }

    public override bool UpdateAnimation()
    {
        return !actorAnimator ||
               !actorAnimator.GetBool(animationName) && !actorAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    public override bool CanRunAnimationsConcurrently(TurnAnimation anim)
    {
        return false;
    }
}
