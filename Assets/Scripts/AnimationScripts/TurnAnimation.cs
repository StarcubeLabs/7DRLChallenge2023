public abstract class TurnAnimation
{
    public abstract void StartAnimation();

    public abstract bool UpdateAnimation();

    public abstract bool CanRunAnimationsConcurrently(TurnAnimation anim);
}
