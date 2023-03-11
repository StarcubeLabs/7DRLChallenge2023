using System;
using UnityEngine;

public class AnimatorAnimation : TurnAnimation
{
    private Animator actorAnimator;
    private string animationName;
    private Action onStart;
    private AudioSource audioSource;

    public AnimatorAnimation(Animator actorAnimator, string animationName, AudioSource audioSource = null)
    {
        this.actorAnimator = actorAnimator;
        this.animationName = animationName;
        this.audioSource = audioSource;
    }

    public AnimatorAnimation(Animator actorAnimator, string animationName, Action onStart, AudioSource audioSource = null)
    {
        this.actorAnimator = actorAnimator;
        this.animationName = animationName;
        this.onStart = onStart;
        this.audioSource = audioSource;
    }
    
    public override void StartAnimation()
    {
        actorAnimator?.SetTrigger(animationName);
        audioSource?.Play();
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
