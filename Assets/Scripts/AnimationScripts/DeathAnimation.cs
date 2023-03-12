using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class DeathAnimation : TurnAnimation
{
    private ActorController actor;
    private Animator actorAnimator;
    private string animationName;
    private EventHandler onDie;
    private AudioSource audioSource;
    
    public DeathAnimation(ActorController actor, Animator actorAnimator, string animationName, EventHandler onDie, AudioSource audioSource = null)
    {
        this.actor = actor;
        this.actorAnimator = actorAnimator;
        this.animationName = animationName;
        this.onDie = onDie;
        this.audioSource = audioSource;
    }
    
    public override void StartAnimation()
    {
        actorAnimator?.SetTrigger(animationName);
        audioSource?.Play();
    }

    public override bool UpdateAnimation()
    {
        if (actorAnimator)
        {
            AnimatorStateInfo state = actorAnimator.GetCurrentAnimatorStateInfo(0);
            return !actorAnimator.GetBool(animationName) && state.IsName(animationName) &&
                   actorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
        }

        return true;
    }
    
    public override void EndAnimation()
    {
        if (onDie == null)
        {
            actor.TriggerDeathVFX();
            Object.Destroy(actor.gameObject);
        }
        else
        {
            onDie(this, EventArgs.Empty);
        }
    }
}
