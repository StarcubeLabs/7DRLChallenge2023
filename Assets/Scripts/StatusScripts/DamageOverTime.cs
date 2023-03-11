using RLDataTypes;

public abstract class DamageOverTime : Status
{
    private int damage;
    private int damageInterval;
    private int turnDamage;
    
    public DamageOverTime(StatusType type, ActorController actor, int turnsLeft, int damage, int damageInterval) : base(type, actor, turnsLeft)
    {
        this.damage = damage;
        this.damageInterval = damageInterval;
        turnDamage = damageInterval;
    }
    
    protected override void OnTurnEnd()
    {
        if (--turnDamage <= 0)
        {
            actor.Hurt(damage);
            turnDamage = damageInterval;
        }
    }
}