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

    /// <summary>
    /// Items on the Map. (NOTE: I know we could simplify this by making an IEntity for both ActorController, and Items...
    /// BUT to not step on toes, just writing this.)
    /// </summary> 
    public List<IInteractable> interactables = new List<IInteractable>();
    
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

    public bool isInteractableInPosition(Vector3Int position)
    {
        for (int i = 0; i < interactables.Count; i++)
        {
            if (interactables[i].gridPosition == position)
            {
                return true;
            }
        }
        return false;
    }

    public ActorController getEntityInPosition(Vector3Int position)
    {
        ActorController entity = null;
        for (int i = 0; i < actors.Count; i++)
        {
            if (actors[i].gridPosition == position)
            {
                entity = actors[i];
            }
        }
        return entity;
    }

    public IInteractable getInteractableInPosition(Vector3Int position)
    {
        IInteractable entity = null;
        for (int i = 0; i < interactables.Count; ++i)
        {
            if (interactables[i].gridPosition == position)
            {
                entity = interactables[i];
            }
        }
        return entity;
    }
}
