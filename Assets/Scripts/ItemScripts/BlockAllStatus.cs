using RLDataTypes;
using UnityEngine;

public class BlockAllStatus : BaseAccessory
{
    public override bool AllowStatus(ActorController target, StatusType statusType)
    {
        return statusType != StatusType.Blindness &&
               statusType != StatusType.Burn &&
               statusType != StatusType.Confusion &&
               statusType != StatusType.Frozen &&
               statusType != StatusType.Muteness &&
               statusType != StatusType.Petrify &&
               statusType != StatusType.Poison &&
               statusType != StatusType.SeismicShock &&
               statusType != StatusType.Sleep &&
               statusType != StatusType.Slow &&
               statusType != StatusType.Stun;
    }
}
