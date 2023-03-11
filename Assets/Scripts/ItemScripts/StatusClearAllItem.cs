using RLDataTypes;

public class StatusClearAllItem : ItemData
{
    public override bool OnConsume(ActorController consumer, Item item)
    {
        ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{consumer.GetDisplayName()} consumed the {ItemName}."));
        consumer.PlayEatAnimation();
        consumer.CureStatuses(
            StatusType.Blindness,
            StatusType.Burn,
            StatusType.Confusion,
            StatusType.Frozen,
            StatusType.Muteness,
            StatusType.Poison,
            StatusType.SeismicShock,
            StatusType.Slow);
        consumer.AddFood(5);
        return true;
    }
}