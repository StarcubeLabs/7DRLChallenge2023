using RLDataTypes;
using UnityEngine;

public class AttackUpAccessory : BaseAccessory
{
    [SerializeField]
    private int damageIncrease;
    
    public override int ModifyDamageUser(int damage, ActorController user, ActorController target)
    {
        return damage + damageIncrease;
    }
}
