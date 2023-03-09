using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropMenu_CurrentItemDisplay : MonoBehaviour
{
    private const string STACK_SIZE_FORMAT = "D2";
    
    public Item CurrentItem;

    public Image ItemPreview;
    public TextMeshProUGUI ItemStackText;
    public int CurrentItemStack;

    public void SetItem(Item newItem)
    {
        CurrentItem = newItem;
        ItemPreview.sprite = CurrentItem.ItemIcon;
        ItemPreview.color = Color.white;
        CurrentItemStack = CurrentItem.CurrentNumberOfStacks;
        ItemStackText.text = CurrentItemStack.ToString(STACK_SIZE_FORMAT);
    }
    
    public void Lower()
    {
        var entityManager = GameObject.FindObjectOfType<EntityManager>();
        entityManager.actors[0].Inventory.ItemToDrop = CurrentItem;
        --CurrentItemStack;
        if (CurrentItemStack < 0)
        {
            CurrentItemStack = 0;
        }
        entityManager.actors[0].Inventory.NumberOfItemsToDrop = CurrentItemStack;
        ItemStackText.text = CurrentItemStack.ToString(STACK_SIZE_FORMAT);
    }

    public void Increase()
    {
        var entityManager = GameObject.FindObjectOfType<EntityManager>();
        entityManager.actors[0].Inventory.ItemToDrop = CurrentItem;
        ++CurrentItemStack;
        if (CurrentItemStack > CurrentItem.CurrentNumberOfStacks)
        {
            CurrentItemStack = CurrentItem.CurrentNumberOfStacks;
        }
        entityManager.actors[0].Inventory.NumberOfItemsToDrop = CurrentItemStack;
        ItemStackText.text = CurrentItemStack.ToString(STACK_SIZE_FORMAT);
    }
    
}
