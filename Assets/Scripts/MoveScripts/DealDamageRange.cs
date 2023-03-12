using UnityEngine;

public class DealDamageRange : MoveData
{
    [SerializeField]
    private int range;
    
    public override void UseMove(ActorController user)
    {
        ActorController target = GetEntityInRange(user);
        if (target)
        {
            DamageTarget(user, target);
        }
    }

    public override bool InAIRange(ActorController user, ActorController target)
    {
        return GetEntityInRange(user) == target;
    }

    private ActorController GetEntityInRange(ActorController user)
    {
        for (int i = 1; i <= range; i++)
        {
            Vector3Int attackPosition = user.GetPositionInFront(i);
            if (!ServicesManager.LevelManager.GetActiveLevel().IsCellWalkable(attackPosition))
            {
                return null;
            }
            
            ActorController target = ServicesManager.EntityManager.getEntityInPosition(attackPosition);
            if (target && !target.Dead)
            {
                return target;
            }
        }

        return null;
    }

    public override void TriggerVFX(ActorController user)
    {
        ActorController target = GetEntityInRange(user);
        if (launchVFX)
        {
            ParticleSystem launchVFXInst = GameObject.Instantiate<ParticleSystem>(launchVFX, user.transform, false);
            Vector3Int frontCell = user.GetPositionInFront();
        }
        if (hitVFX)
        {
            GameObject.Instantiate<ParticleSystem>(launchVFX, user.transform, false);
                //Hit VFX
            ParticleSystem p = GameObject.Instantiate<ParticleSystem>(hitVFX, target.transform, false);
            p.transform.localPosition = Vector3.zero;
        }
    }
}
