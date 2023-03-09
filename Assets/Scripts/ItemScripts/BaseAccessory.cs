public class BaseAccessory : EquippableItem
{
    public override bool OnConsume(ActorController consumer, Item item)
    {
        consumer.EquipAccessory(item);
        return false;
    }

    public override void OnDrop(ActorController owner, Item item)
    {
        owner.UnequipAccessory(item);
    }
}
