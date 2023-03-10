using UnityEngine;
using RLDataTypes;

public class StatusClearItem : ItemData
{
    [SerializeField]
    private StatusType statusToClear = StatusType.None;
    
    public override bool OnConsume(ActorController consumer, Item item)
    {
        ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{consumer.GetDisplayName()} consumed the {ItemName}."));
        consumer.PlayEatAnimation();
        consumer.CureStatus(statusToClear);
        consumer.AddFood(5);
        return true;
    }
}
