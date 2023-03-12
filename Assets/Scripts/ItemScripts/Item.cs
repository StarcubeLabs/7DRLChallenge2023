using System;
using UnityEngine;

public class Item : EntityController, IInteractable
{
    [HideInInspector]
    public EntityManager entityManager;
    [HideInInspector]
    public Grid grid;
    public DungeonMap testMap;
    public ItemData ItemData;
    public ActorController Owner;

    public string ItemName => ItemData.ItemName;

    public string ItemDescription => ItemData.ItemDescription;

    public Sprite ItemIcon => ItemData.ItemIcon;

    public GameObject ItemObject => ItemData.ItemObject;

    public bool CanBeStacked => ItemData.IsStackable;


    public int MaximumNumberOfStacks => ItemData.MaximumStack;
    public int CurrentNumberOfStacks = 0;
    
    
    public virtual void Interact(ActorController interactingEntity)
    {
        if (gameObject.activeInHierarchy)
        {
            entityManager = GameObject.FindObjectOfType<EntityManager>();
            if (interactingEntity.Inventory.AddItem(this))
            {
                ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{interactingEntity.GetDisplayName()} picked up the {ItemData.ItemName}."));
                Owner = interactingEntity;
                gameObject.SetActive(false);
                entityManager.RemoveItem(this);
            }
            else
            {
                ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{interactingEntity.GetDisplayName()} stepped on the {ItemData.ItemName}."));
            }
        }
    }

    void Start()
    {
        if (!ItemObject && GetComponent<SpriteRenderer>())
        {
            GetComponent<SpriteRenderer>().sprite = ItemIcon;
        }
    }

    public override void SaveEntity(EntityManager entityManager)
    {
        entityManager.interactables.Remove(this);
    }

    public override void LoadEntity(EntityManager entityManager)
    {
        entityManager.interactables.Add(this);
    }

    public void SnapToPosition(Vector3Int gridPosition)
    {
        grid = FindObjectOfType<Grid>();

        //Disable pure location snap so "faux-animation" lerping can work instead.
        Vector3 worldPosition = grid.GetCellCenterWorld(gridPosition);
        this.transform.position = new Vector3(worldPosition.x, 0, worldPosition.z);
    }

    public bool OnDrop()
    {
        if (Owner.Inventory.DropItem(this, Owner.gridPosition))
        {
            ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{Owner.GetDisplayName()} dropped the {ItemData.ItemName}."));
            ItemData.OnDrop(Owner, this);
            return true;
        }

        return false;
    }

    public void GenerateRandomStackSize()
    {
        CurrentNumberOfStacks = 1;
    }

    public bool Consume()
    {
        if (ItemData.OnConsume(Owner, this))
        {
            bool depleted = false;
            if (CanBeStacked)
            {
                if (--CurrentNumberOfStacks == 0)
                {
                    depleted = true;
                }
            }
            else
            {
                depleted = true;
            }
            if (depleted)
            {
                int itemIndex = Array.IndexOf(Owner.Inventory.Items, this);
                Owner.Inventory.Items[itemIndex] = null;
                Destroy(gameObject);
                return true;
            }
        }
        return false;
    }
}
