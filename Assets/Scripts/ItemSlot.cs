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

    public Item itemSlot;

    public TextMeshProUGUI StackCountText;

    public void SetInventory(Inventory parentInventory)
    {
        inventory = parentInventory;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (itemSlot == null)
        {
            StackCountText.gameObject.SetActive(false);
        }
        else
        {
            if (!itemSlot.CanBeStacked)
            {
                StackCountText.gameObject.SetActive(false);
            }
            else
            {
                StackCountText.text = itemSlot.CurrentNumberOfStacks.ToString("D2");
            }
        }

        if (itemSlot != null)
        {
            if (ItemPreview != null)
            {
                ItemPreview.GetComponent<Image>().color = Color.white;
                ItemPreview.GetComponent<Image>().sprite = itemSlot.ItemIcon;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (itemSlot.CanBeStacked)
            {
                var dropMenu = GameObject.FindObjectOfType<DropItemMenu>();
                dropMenu.GetComponent<DropItemMenu>().CurrentItemDisplay.SetItem(itemSlot);
                dropMenu.GetComponent<DropItemMenu>().CurrentDropItemDisplay.SetItem(itemSlot);
                dropMenu.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                itemSlot.OnDrop();
            }
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (itemSlot is IConsumable)
            {
                (itemSlot as IConsumable).OnConsume();
            }
        }
        inventoryDrawerReference.Draw();
    }
}
