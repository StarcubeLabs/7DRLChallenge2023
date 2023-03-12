using UnityEngine;

public class Cure : MoveData
{
    [SerializeField]
    private int healAmount;
    
    public override void UseMove(ActorController user)
    {
        user.HealAmount(healAmount);
    }
}
