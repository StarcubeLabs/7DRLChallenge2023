using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLDataTypes;

public class HealRouletteType : MoveData
{
    [SerializeField]
    private StatusType statusType = StatusType.None;


    public override void UseMove(ActorController user)
    {
        if(Random.Range(0, 100) > 50)
        {
            user.ApplyModifier(StatusModifier.Regeneration, 4);
        }
        else
        {
            user.ApplyStatus(StatusType.Poison, 3);
        }
    }
}
