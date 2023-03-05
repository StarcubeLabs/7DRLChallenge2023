using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Item : MonoBehaviour, IInteractable
{
    public EntityManager entityManager;
    public Grid grid;
    public TestMap testMap;
    public ItemData ItemData;

    public string ItemName => ItemData.ItemName;

    public string ItemDescription => ItemData.ItemDescription;

    public long ItemValue => ItemData.ItemValue;

    public Sprite ItemIcon => ItemData.ItemIcon;

    public bool CanBeStacked => ItemData.IsStackable;


    public int MaximumNumberOfStacks => ItemData.MaximumStack;
    public int CurrentNumberOfStacks = 0;
    
    
    public virtual void Interact(ActorController interactingEntity)
    {
        throw new System.NotImplementedException();
    }

    public Vector3Int gridPosition 
    { 
        get; 
        set; 
    }

    void Start()
    {
        if (GetComponent<SpriteRenderer>())
        {
            GetComponent<SpriteRenderer>().sprite = ItemIcon;
        }
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
