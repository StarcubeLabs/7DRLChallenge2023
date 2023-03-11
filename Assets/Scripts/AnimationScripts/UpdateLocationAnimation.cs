using UnityEngine;

public abstract class UpdateLocationAnimation : TurnAnimation
{
    protected ActorController actor;
    protected Animator actorAnimator;
    private float animationTime;
    private float animationTimer;
    
    public UpdateLocationAnimation(ActorController actor, Animator actorAnimator, float animationTime)
    {
        this.actor = actor;
        this.actorAnimator = actorAnimator;
        this.animationTime = animationTime;
    }

    public override bool UpdateAnimation()
    {
        animationTimer += Time.deltaTime;
        return actor.UpdateVisualLocation(animationTimer / animationTime);
    }
}
