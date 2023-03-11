using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class DeathAnimation : TurnAnimation
{
    private ActorController actor;
    private Animator actorAnimator;
    private string animationName;
    private EventHandler onDie;
    
    public DeathAnimation(ActorController actor, Animator actorAnimator, string animationName, EventHandler onDie)
    {
        this.actor = actor;
        this.actorAnimator = actorAnimator;
        this.animationName = animationName;
        this.onDie = onDie;
    }
    
    public override void StartAnimation()
    {
        actorAnimator?.SetTrigger(animationName);
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
            Object.Destroy(actor.gameObject);
        }
        else
        {
            onDie(this, EventArgs.Empty);
        }
    }
}
