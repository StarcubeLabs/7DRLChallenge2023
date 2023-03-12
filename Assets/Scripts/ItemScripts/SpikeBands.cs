using UnityEngine;

public class SpikeBands : BaseAccessory
{
    [SerializeField]
    [Range(0, 1)]
    private float recoilPercentage;

    public override void OnActorAttacked(ActorController attacker, int damage)
    {
        attacker.Hurt(Mathf.Max(1, (int)(damage * recoilPercentage)));
    }
}
