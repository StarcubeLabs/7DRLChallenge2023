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

    private void Update()
    {
        if (turnOrder.Count > 0 && !CanCurrentActorMove())
        {
            turnOrder[0].EndTurn(true);
        }
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
        if (turnOrder.Count == 0)
        {
            return false;
        }
        return CanCurrentActorMove() && turnOrder[0] == actorController;
    }

    public void KickToBackOfTurnOrder(ActorController actorController)
    {
        turnOrder.Remove(actorController);
        turnOrder.Insert(turnOrder.Count, actorController);
    }

    private bool CanCurrentActorMove()
    {
        if (turnOrder.Count == 0)
        {
            return false;
        }

        return !turnOrder[0].IsImmobilized();
    }
}
