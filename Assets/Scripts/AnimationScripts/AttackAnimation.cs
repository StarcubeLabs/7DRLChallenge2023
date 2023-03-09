using UnityEngine;

public class AttackAnimation : TurnAnimation
{
    private Animator actorAnimator;
    private string animationName;
    
    public AttackAnimation(Animator actorAnimator, string animationName)
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
        if (!actorAnimator)
        {
            return true;
        }

        return !actorAnimator.GetBool(animationName) && !actorAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    public override bool CanRunAnimationsConcurrently(TurnAnimation anim)
    {
        return false;
    }
}
