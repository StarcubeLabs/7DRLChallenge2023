using RLDataTypes;
using UnityEngine;

public class ApplyStatusItem : ItemData
{
    [SerializeField]
    private StatusType status = StatusType.None;
    [SerializeField]
    private int statusTurnCount;
    
    public override bool OnConsume(ActorController consumer, Item item)
    {
        ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{consumer.GetDisplayName()} consumed the {ItemName}."));
        consumer.PlayEatAnimation();
        consumer.ApplyStatus(status, statusTurnCount);
        consumer.AddFood(5);
        return true;
    }
}
