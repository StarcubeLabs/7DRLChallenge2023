public class FoodItem : ItemData
{
    public int FoodAmount;

    public override bool OnConsume(ActorController consumer, Item item)
    {
        ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{consumer.GetDisplayName()} consumed the {ItemName}."));
        consumer.PlayEatAnimation();
        consumer.AddFood(FoodAmount, string.Format("{0} ate {1} points worth of food!", consumer.GetDisplayName(), FoodAmount));
        return true;
    }
}
