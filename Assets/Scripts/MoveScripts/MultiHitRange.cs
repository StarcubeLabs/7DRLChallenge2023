using UnityEngine;

public class MultiHitRange : DealDamageRange
{
    [SerializeField]
    private int minHits;
    [SerializeField]
    private int maxHits;
    [SerializeField]
    private int powerVariance;
    
    public override void UseMove(ActorController user)
    {
        for (int i = 0; i < Random.Range(minHits, maxHits + 1); i++)
        {
            base.UseMove(user);
        }
    }

    public override int GetMovePower()
    {
        return Random.Range(Power - powerVariance, Power + powerVariance + 1);
    }
}
