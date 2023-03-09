public abstract class TurnAnimation
{
    public virtual void StartAnimation()
    {
    }

    public abstract bool UpdateAnimation();

    public virtual void EndAnimation()
    {
    }

    public abstract bool CanRunAnimationsConcurrently(TurnAnimation anim);
}
