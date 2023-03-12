using TMPro;
using UnityEngine;

public class InventoryDrawer : MonoBehaviour
{
    private EntityManager entityManager;
    public ActorController player;
    public Inventory PlayerInventory;
    public ItemSlot ItemSlotPrefab;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    private GameObject tooltipObject { get { return itemNameText.transform.parent.gameObject; } }
    
    void Awake()
    {
        entityManager = FindObjectOfType<EntityManager>();
    }
    
    void OnEnable()
    {
        player = entityManager.actors[0];
        PlayerInventory = player.Inventory;
        if (PlayerInventory != null)
        {
            Draw();
        }
        HideTooltip();
    }
    
    public void Draw()
    {
        //Debug.Log("INVENTORY DRAWN");
        for (int i = 0; i < transform.childCount; ++i)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }

        for(int i=0;i<PlayerInventory.NumberOfSlots;++i)
        {
            ItemSlot inventorySlot = Instantiate(ItemSlotPrefab);
            inventorySlot.SetInventory(PlayerInventory);
            inventorySlot.inventoryDrawerReference = this;
            inventorySlot.transform.SetParent(gameObject.transform);
            if (PlayerInventory.Items[i] != null)
            {
                inventorySlot.item = PlayerInventory.Items[i];
            }
        }
    }

    public void Open()
    {
        transform.parent.gameObject.SetActive(true);
        tooltipObject.SetActive(true);
    }

    public void CloseAndEndTurn()
    {
        Close();
        ServicesManager.HudManager.ContextMenu.Close();
        player.EndTurn();
    }

    public void Close()
    {
        HideTooltip();
        tooltipObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    public void SetTooltip(Item item)
    {
        itemNameText.text = item.ItemData.ItemName;
        itemDescriptionText.text = item.ItemData.ItemDescription;
    }

    public void HideTooltip()
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";
    }
}
