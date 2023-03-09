public class WalkAnimation : TurnAnimation
{
    private ActorController actor;
    
    public WalkAnimation(ActorController actor)
    {
        this.actor = actor;
    }
    
    public override void StartAnimation()
    {
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
