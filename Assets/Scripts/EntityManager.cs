using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EntityManager: MonoBehaviour
{
    public List<ActorController> actors = new List<ActorController>();

    public EventHandler<ActorController> onAddActor;
    public EventHandler<ActorController> onRemoveActor;

    public void AddActor(ActorController actorController)
    {
        actors.Add(actorController);
        if (onAddActor != null)
        {
            onAddActor(this, actorController);
        }
    }

    public void RemoveActor(ActorController actorController)
    {
        actors.Remove(actorController);
        if (onRemoveActor != null)
        {
            onRemoveActor(this, actorController);
        }
    }

    public bool isEntityInPosition(Vector3Int position)
    {
        for (int i = 0; i < actors.Count; i++)
        {
            if (actors[i].gridPosition == position)
            {
                return true;
            }
        }
        return false;
    }
}
