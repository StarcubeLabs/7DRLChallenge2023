using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Item : EntityController, IInteractable
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

    public long ItemValue => ItemData.ItemValue;

    public Sprite ItemIcon => ItemData.ItemIcon;

    public bool CanBeStacked => ItemData.IsStackable;


    public int MaximumNumberOfStacks => ItemData.MaximumStack;
    public int CurrentNumberOfStacks = 0;
    
    
    public virtual void Interact(ActorController interactingEntity)
    {
        if (gameObject.activeInHierarchy)
        {
            entityManager = GameObject.FindObjectOfType<EntityManager>();
            interactingEntity.Inventory.AddItem(this);
            Owner = interactingEntity;
            gameObject.SetActive(false);
            entityManager.RemoveItem(this);
        }
    }

    void Start()
    {
        if (GetComponent<SpriteRenderer>())
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

    public void OnDrop()
    {
        entityManager = FindObjectOfType<EntityManager>();
        entityManager.actors[0].Inventory.DropItem(this,entityManager.actors[0].gridPosition);
    }
    
}
