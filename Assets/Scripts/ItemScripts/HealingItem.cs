public class HealingItem : ItemData
{
    public int HealAmount;

    public override bool OnConsume(ActorController consumer, Item item)
    {
        consumer.HealAmount(HealAmount);
        return true;
    }
}
