using UnityEngine;

public class ApplyStatusSelfMove : MoveData
{
    public override void UseMove(ActorController user)
    {
        user.ApplyStatus(afflictionType, afflictionTurnCount);
    }
    
    public override bool UsableByAI(ActorController user, ActorController target)
    {
        return !user.HasStatus(afflictionType);
    }

    public override bool InAIRange(ActorController user, ActorController target)
    {
        return true;
    }
}
