using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLDataTypes;

public class StatusClearItem : ItemData
{
    StatusType statusToClear = StatusType.None;
    public override bool OnConsume(ActorController consumer, Item item)
    {
        //consumer
        return true;
    }
}
