using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLDataTypes;

public class Roulette : MoveData
{
    public override void UseMove(ActorController user)
    {
        if (Random.value < afflictionChance)
        {
            user.ApplyStatus(StatusType.Regeneration, 4);
        }
        else
        {
            user.ApplyStatus(StatusType.Poison, 2);
        }
    }
    
    public override bool UsableByAI(ActorController user, ActorController target)
    {
        return !user.HasFullHealth;
    }

    public override bool InAIRange(ActorController user, ActorController target)
    {
        return true;
    }
}
