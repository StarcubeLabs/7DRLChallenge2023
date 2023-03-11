using RLDataTypes;
using UnityEngine;

public class SeismicShock : Status
{
    private const float STUN_CHANCE = 0.5f;
    private bool stunned;

    public SeismicShock(ActorController actor, int turnsLeft) : base(StatusType.SeismicShock, actor, turnsLeft)
    {
        HUD_TEXT = "Seismic Shocked";
        RollStunChance();
    }

    protected override void OnTurnEnd()
    {
        RollStunChance();
    }

    public override bool IsImmobilized()
    {
        return stunned;
    }

    private void RollStunChance()
    {
        stunned = Random.value < STUN_CHANCE;
    }

    public override string GetStatusApplyMessage()
    {
        return $"{actor.GetDisplayName()} is suffering from seismic shock!";
    }

    public override string GetStatusCureMessage()
    {
        return $"{actor.GetDisplayName()} is no longer suffering from seismic shock!";
    }
}
