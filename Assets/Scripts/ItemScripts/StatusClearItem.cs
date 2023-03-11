using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLDataTypes;

public class StatusClearItem : ItemData
{
    [SerializeField]
    private StatusType statusToClear = StatusType.None;
    public override bool OnConsume(ActorController consumer, Item item)
    {
        consumer.PlayEatAnimation();
        ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{consumer.GetDisplayName()} consumed the {ItemName}."));
        //consumer
        return true;
    }
}
