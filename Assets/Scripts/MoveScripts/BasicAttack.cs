using RLDataTypes;

public class BasicAttack : DealDamageFront
{
    public override ElementType GetModifiedElement(ActorController user)
    {
        BaseWeapon weapon = user.Weapon;
        return weapon ? weapon.weaponElement : Element;
    }
}
