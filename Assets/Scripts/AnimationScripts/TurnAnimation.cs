public abstract class TurnAnimation
{
    public virtual void StartAnimation()
    {
    }

    public abstract bool UpdateAnimation();

    public virtual void EndAnimation()
    {
    }

    public virtual bool CanRunAnimationsConcurrently(TurnAnimation anim)
    {
        return false;
    }
}
