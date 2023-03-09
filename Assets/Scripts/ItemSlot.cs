using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour,IPointerDownHandler
{
    public Inventory inventory;
    public InventoryDrawer inventoryDrawerReference;
    public GameObject ItemPreview;

    public Item item;

    public TextMeshProUGUI StackCountText;

    public void SetInventory(Inventory parentInventory)
    {
        inventory = parentInventory;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (item == null)
        {
            StackCountText.gameObject.SetActive(false);
        }
        else
        {
            if (!item.CanBeStacked)
            {
                StackCountText.gameObject.SetActive(false);
            }
            else
            {
                StackCountText.text = item.CurrentNumberOfStacks.ToString("D2");
            }
        }

        if (item != null)
        {
            if (ItemPreview != null)
            {
                ItemPreview.GetComponent<Image>().color = Color.white;
                ItemPreview.GetComponent<Image>().sprite = item.ItemIcon;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item != null)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (item.CanBeStacked)
                {
                    var dropMenu = GameObject.FindObjectOfType<DropItemMenu>();
                    dropMenu.GetComponent<DropItemMenu>().CurrentItemDisplay.SetItem(item);
                    dropMenu.GetComponent<DropItemMenu>().CurrentDropItemDisplay.SetItem(item);
                    dropMenu.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (item.OnDrop())
                {
                    inventoryDrawerReference.CloseAndEndTurn();
                }
            }
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (item.Consume())
                {
                    item = null;
                }
                inventoryDrawerReference.CloseAndEndTurn();
            }
        }
        inventoryDrawerReference.Draw();
    }
}
