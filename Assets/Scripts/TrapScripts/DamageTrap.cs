using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrap : BasicTrap
{

    public int HurtAmount = 0;
    
    public override void ActivateTrap(ActorController actor)
    {
        if (Random.Range(0, 1) < successChance)
        {
            actor.Hurt(HurtAmount);
        }
    }
    
}
