using UnityEngine;

public class StaminaBadge : BaseAccessory
{
    [SerializeField]
    private int modifiedHungerDrainRate;
    
    public override int ModifyHungerDrainRate(int hungerDrainRate)
    {
        return modifiedHungerDrainRate;
    }
}
