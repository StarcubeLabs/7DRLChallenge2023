using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TurnManager: MonoBehaviour
{
    List<ActorController> turnOrder = new List<ActorController>();
    EntityManager actorManager;

    public void Awake()
    {
        actorManager = FindObjectOfType<EntityManager>();
        actorManager.onAddActor += OnAddActor;
        actorManager.onRemoveActor += OnRemoveActor;
    }

    public void OnAddActor(object sender, ActorController actorController)
    {
        KickToBackOfTurnOrder(actorController);
    }

    public void OnRemoveActor(object sender, ActorController actorController)
    {
        turnOrder.Remove(actorController);
    }

    public bool CanMove(ActorController actorController)
    {
        if (turnOrder == null || turnOrder.Count == 0)
        {
            return false;
        }
        return turnOrder[0] == actorController;
    }

    public void KickToBackOfTurnOrder(ActorController actorController)
    {
        turnOrder.Remove(actorController);
        turnOrder.Insert(turnOrder.Count, actorController);
    }
}
