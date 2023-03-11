using RLDataTypes;
using UnityEngine;

public class Confusion : Status
{

    public Confusion(ActorController actor, int turnsLeft) : base(StatusType.Confusion, actor, turnsLeft)
    {
        HUD_TEXT = "Confused";
    }

    public override void ModifyFacingDirection()
    {
        //We're confused. Set our direction to RANDOM.
        actor.FaceDirection((ActorController.ActorDirection)Random.Range(0, (int)ActorController.ActorDirection.numDirections));
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} became confused!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} snapped out of confusion!";
    }
}