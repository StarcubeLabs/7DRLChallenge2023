using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropMenu_DropItem : MonoBehaviour
{
    public Item CurrentItem;

    public Image ItemPreview;
    public TextMeshProUGUI ItemStackText;
    public int CurrentItemStack;

    public void SetItem(Item newItem)
    {
        CurrentItem = newItem;
        ItemPreview.sprite = CurrentItem.ItemIcon;
        ItemPreview.color = Color.white;
        CurrentItemStack = 0;
        ItemStackText.text = CurrentItemStack.ToString();
    }
    
    public void Lower()
    {
        --CurrentItemStack;
        if (CurrentItemStack < 0)
        {
            CurrentItemStack = 0;
        }
        var entityManager = GameObject.FindObjectOfType<EntityManager>();
        entityManager.actors[0].Inventory.NumberOfItemsToDrop = CurrentItemStack;
        ItemStackText.text = CurrentItemStack.ToString("D2");
    }

    public void Increase()
    {
        ++CurrentItemStack;
        if (CurrentItemStack > CurrentItem.CurrentNumberOfStacks)
        {
            CurrentItemStack = CurrentItem.CurrentNumberOfStacks;
        }

        var entityManager = GameObject.FindObjectOfType<EntityManager>();
        entityManager.actors[0].Inventory.NumberOfItemsToDrop = CurrentItemStack;
        ItemStackText.text = CurrentItemStack.ToString("D2");
    }
}
