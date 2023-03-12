using UnityEngine;

public class DefenseUpAccessory : BaseAccessory
{
    [SerializeField]
    private int damageDecrease;

    public override int ModifyDamageTarget(int damage, ActorController user, ActorController target)
    {
        return damage - damageDecrease;
    }
}
