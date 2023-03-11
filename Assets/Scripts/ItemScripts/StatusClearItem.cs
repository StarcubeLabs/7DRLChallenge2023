using UnityEngine;
using RLDataTypes;

public class StatusClearItem : ItemData
{
    [SerializeField]
    private StatusType statusToClear = StatusType.None;
    
    public override bool OnConsume(ActorController consumer, Item item)
    {
        consumer.CureStatus(statusToClear);
        return true;
    }
}
