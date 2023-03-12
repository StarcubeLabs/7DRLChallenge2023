using RLDataTypes;
using UnityEngine;

public static class DamageCalculator
{
    public static int CalculateDamage(MoveData move, ActorController user, ActorController target)
    {
        int moveDamage = move.GetMovePower() + user.baseAttackPower;
        ElementType moveElement = move.GetModifiedElement(user);
        if (moveElement == user.GetEffectiveType())
        {
            moveDamage++;
        }

        moveDamage = user.ModifyDamageUser(moveDamage, target);
        moveDamage += GetTypeEffectivenessModifier(moveElement, target.GetEffectiveType());
        moveDamage = target.ModifyDamageTarget(moveDamage, user);
        moveDamage = Mathf.Max(0, moveDamage);
        
        return moveDamage;
    }

    public static int GetTypeEffectivenessModifier(ElementType moveType, ElementType targetType)
    {
        if (moveType == ElementType.Fire && targetType == ElementType.Grass ||
            moveType == ElementType.Grass && targetType == ElementType.Water ||
            moveType == ElementType.Water && targetType == ElementType.Fire)
        {
            return 2;
        }

        if (moveType == ElementType.Fire && targetType == ElementType.Water ||
            moveType == ElementType.Grass && targetType == ElementType.Fire ||
            moveType == ElementType.Water && targetType == ElementType.Grass)
        {
            return -2;
        }

        return 0;
    }
}
