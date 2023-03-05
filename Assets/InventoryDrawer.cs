using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDrawer : MonoBehaviour
{
    private EntityManager entityManager;
    public Inventory PlayerInventory;
    public ItemSlot ItemSlotPrefab;
    
    void Awake()
    {
        entityManager = FindObjectOfType<EntityManager>();
    }
    
    void OnEnable()
    {
        PlayerInventory = entityManager.actors[0].Inventory;
        if (PlayerInventory != null)
        {
            Draw();
        }
    }
    
    public void Draw()
    {
        Debug.Log("INVENTORY DRAWN");
        for (int i = 0; i < transform.childCount; ++i)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }

        for(int i=0;i<PlayerInventory.NumberOfSlots;++i)
        {
            ItemSlot inventorySlot = Instantiate(ItemSlotPrefab);
            inventorySlot.SetInventory(PlayerInventory);
            inventorySlot.inventoryDrawerReference = this;
            inventorySlot.transform.parent = gameObject.transform;
            if (PlayerInventory.Items[i] != null)
            {
                inventorySlot.itemSlot = PlayerInventory.Items[i];
            }
        }

    }
    
}
