public class UpdateHitPoints : TurnAnimation
{
    private ActorController actor;
    private int hitPoints;
    
    public UpdateHitPoints(ActorController actor, int hitPoints)
    {
        this.actor = actor;
        this.hitPoints = hitPoints;
    }

    public override bool UpdateAnimation()
    {
        return true;
    }

    public override void EndAnimation()
    {
        actor.visualHitPoints = hitPoints;
    }
}
