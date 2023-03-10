public class HealingItem : ItemData
{
    public int HealAmount;

    public override bool OnConsume(ActorController consumer, Item item)
    {
        ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{consumer.GetDisplayName()} consumed the {ItemName}."));
        consumer.PlayEatAnimation();
        consumer.HealAmount(HealAmount);
        return true;
    }
}
