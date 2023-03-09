using System;
using UnityEngine;

public class AnimatorAnimation : TurnAnimation
{
    private Animator actorAnimator;
    private string animationName;
    private Action onStart;

    public AnimatorAnimation(Animator actorAnimator, string animationName)
    {
        this.actorAnimator = actorAnimator;
        this.animationName = animationName;
    }

    public AnimatorAnimation(Animator actorAnimator, string animationName, Action onStart)
    {
        this.actorAnimator = actorAnimator;
        this.animationName = animationName;
        this.onStart = onStart;
    }
    
    public override void StartAnimation()
    {
        actorAnimator?.SetTrigger(animationName);
        if (onStart != null)
        {
            onStart();
        }
    }

    public override bool UpdateAnimation()
    {
        return !actorAnimator ||
               !actorAnimator.GetBool(animationName) && !actorAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
}
