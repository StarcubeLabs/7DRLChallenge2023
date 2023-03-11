using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPointItem : ItemData
{
    public int RestoreAmount = 5;

    // Start is called before the first frame update
    public override bool OnConsume(ActorController consumer, Item item)
    {
        ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{consumer.GetDisplayName()} consumed the {ItemName}."));
        consumer.PlayEatAnimation();
        consumer.PowerRestore(RestoreAmount);
        consumer.AddFood(5);
        return true;
    }
}
