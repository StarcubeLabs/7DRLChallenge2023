using RLDataTypes;
using UnityEngine;

public class AttackUpTypeAccessory : BaseAccessory
{
    [SerializeField]
    private int damageIncrease;
    [SerializeField]
    private ElementType elementType;
    
    public override int ModifyDamageUser(int damage, ActorController user, ActorController target)
    {
        if (target.GetEffectiveType() == elementType)
        {
            return damage + damageIncrease;
        }
        return damage;
    }
}
