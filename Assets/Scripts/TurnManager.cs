using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TurnManager: MonoBehaviour
{
    List<ActorController> actors = new List<ActorController>();

    public void Register(ActorController actorController)
    {
        actors.Add(actorController);
    }

    public bool CanMove(ActorController actorController)
    {
        return actors[0] == actorController;
    }

    public void KickToBackOfTurnOrder(ActorController actorController)
    {
        actors.Remove(actorController);
        actors.Insert(actors.Count, actorController);
    }
}
