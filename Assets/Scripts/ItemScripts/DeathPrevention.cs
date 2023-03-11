public class DeathPrevention : ItemData
{
    public override bool OnConsume(ActorController consumer, Item item)
    {
        return false;
    }

    public override bool PreventDeath(ActorController owner, Item item)
    {
        owner.hitPoints.x = owner.hitPoints.y;
        owner.hunger.x = owner.hunger.y;
        foreach (Move move in owner.moves)
        {
            move.pp = move.moveData.MaxPP;
        }
        owner.UpdateVisualHitPoints();
        owner.UpdateVisualHunger();
        ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{owner.GetDisplayName()} was revived!"));
        owner.PlayEatAnimation();
        return true;
    }
}
