using RLDataTypes;
using UnityEngine;

public class DefenseUpTypeAccessory : BaseAccessory
{
    [SerializeField]
    private int damageDecrease;
    [SerializeField]
    private ElementType elementType;

    public override int ModifyDamageTarget(int damage, ActorController user, ActorController target)
    {
        if (user.GetEffectiveType() == elementType)
        {
            return damage - damageDecrease;
        }
        return damage;
    }
}
