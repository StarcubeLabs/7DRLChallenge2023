public class UpdateHunger: TurnAnimation
{
    private ActorController actor;
    private int hunger;
    
    public UpdateHunger(ActorController actor, int hunger)
    {
        this.actor = actor;
        this.hunger = hunger;
    }

    public override bool UpdateAnimation()
    {
        return true;
    }

    public override void EndAnimation()
    {
        actor.visualHungerPoints = hunger;
    }
}
