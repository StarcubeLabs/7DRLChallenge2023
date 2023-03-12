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

    public override void TriggerVFX(ActorController user)
    {
        if (launchVFX != null)
        {
            GameObject.Instantiate<ParticleSystem>(launchVFX, user.transform, false);
        }
    }
}
