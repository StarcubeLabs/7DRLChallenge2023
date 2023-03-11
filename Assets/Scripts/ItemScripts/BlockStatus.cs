using RLDataTypes;
using UnityEngine;

public class BlockStatus : BaseAccessory
{
    [SerializeField]
    private StatusType statusBlockType = StatusType.None;
    
    public override bool AllowStatus(ActorController target, StatusType statusType)
    {
        return statusType != statusBlockType;
    }
}
