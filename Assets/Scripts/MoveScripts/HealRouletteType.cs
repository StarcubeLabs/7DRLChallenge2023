using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLDataTypes;

public class HealRouletteType : MoveData
{
    public override void UseMove(ActorController user)
    {
        if(Random.Range(0, 100) > 50)
        {
            user.ApplyModifier(StatusType.Regeneration, 4);
        }
        else
        {
            user.ApplyStatus(StatusType.Poison, 3);
        }
    }
}
