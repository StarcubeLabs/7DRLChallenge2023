using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContextMenu: MonoBehaviour, IMenuInteractable
{
    public EventSystem eventSystem;
    public Image cursor;

    public ElementGroup contextMainMenu;

    public MoveMenu moveMenu;
    public Transform inventoryMenu;
    public Transform infoMenu;
    public Transform gameMenu;

    public MenuItem moveMenuItem;
    public MenuItem inventoryMenuItem;
    public MenuItem infoMenuItem;
    public MenuItem gameMenuItem;

    public void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        
        Array.ForEach(GetComponentsInChildren<MenuItem>(), (menuItem) =>
        {
            menuItem.AttachMenuListener(this);
        });

        moveMenuItem.onSelect += OnSelectMoveMenu;
        moveMenu.onChooseMove += OnChooseMove;

        inventoryMenuItem.onSelect += OnSelectInventory;
    }

    public void OnChooseMove(object sender, EventArgs args)
    {
        Close();
    }

    public void OnCancelUseMove()
    {
        contextMainMenu.Show();
        NavigateToFirstMenuItem();
    }

    public void OnCancelTeachMove()
    {
        ServicesManager.HudManager.ContextMenu.OpenInventory();
    }

    public void StopHighlightMenuItem(MenuItem sender)
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            cursor.enabled = false;
        }
    }

    public void StartHighlightMenuItem(MenuItem sender)
    {
        if (IsMenuOpen())
        {
            RectTransform menuItemRect = (RectTransform)(sender.transform);
            cursor.rectTransform.position = new Vector2(-20 + menuItemRect.rect.xMin, 0) + new Vector2(menuItemRect.position.x, menuItemRect.position.y);
            cursor.enabled = true;
        }
    }

    public void OnSelectMoveMenu(object sender, EventArgs eventArgs)
    {
        moveMenu.SetupUseMoveMenu(this);
        contextMainMenu.Hide();

        NavigateToFirstMenuItem();
    }

    public void OpenTeachMoveMenu(Move newMove, Action consumeAction)
    {
        CloseInventory();
        moveMenu.SetupTeachMoveMenu(this, newMove, consumeAction);

        NavigateToFirstMenuItem();
    }

    public void OnSelectInventory(object sender, EventArgs eventArgs)
    {
        OpenInventory();
        contextMainMenu.Hide();
    }

    public void OpenInventory()
    {
        inventoryMenu.gameObject.SetActive(true);
        inventoryMenu.GetComponentInChildren<InventoryDrawer>().Open();
        cursor.enabled = false;
    }

    public void OpenMenu()
    {
        contextMainMenu.Show();

        Array.ForEach(moveMenu.GetComponentsInChildren<MenuItem>(), (menuItem) =>
        {
            menuItem.AttachMenuListener(this);
        });

        ServicesManager.HudManager.MessageBox.Hide();

        NavigateToFirstMenuItem();
    }

    public void NavigateToFirstMenuItem()
    {
        Transform root = getActiveMenu();
        MenuItem firstMenuItem = root.GetComponentInChildren<MenuItem>(true);
        GameObject menuItem = firstMenuItem.gameObject;
        eventSystem.firstSelectedGameObject = menuItem;
        eventSystem.SetSelectedGameObject(menuItem);

        StartHighlightMenuItem(firstMenuItem);
    }

    public void NavigateDown()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            NavigateToFirstMenuItem();
        }
        else
        {
            AxisEventData moveEventData = new AxisEventData(EventSystem.current);
            moveEventData.moveDir = MoveDirection.Down;
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, moveEventData, ExecuteEvents.moveHandler);
        }
    }

    public void NavigateUp()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            NavigateToFirstMenuItem();
        }
        else
        {
            AxisEventData moveEventData = new AxisEventData(EventSystem.current);
            moveEventData.moveDir = MoveDirection.Up;
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, moveEventData, ExecuteEvents.moveHandler);
        }
    }

    public void NavigateBack()
    {
        if (eventSystem.currentSelectedGameObject != null)
        {
            AxisEventData moveEventData = new AxisEventData(EventSystem.current);
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, moveEventData, ExecuteEvents.cancelHandler);
        }
        if (moveMenu.elementGroup.enabled)
        {
            moveMenu.Close();
            moveMenu.Cancel();
        }
        else if (inventoryMenu.gameObject.activeSelf)
        {
            CloseInventory();
            contextMainMenu.Show();
            NavigateToFirstMenuItem();
        }
        else if (contextMainMenu.enabled)
        {
            contextMainMenu.Hide();
            Close();
        }
    }

    public bool IsMenuOpen()
    {
        return moveMenu.elementGroup.enabled || contextMainMenu.enabled || inventoryMenu.gameObject.activeSelf;
    }

    public void Close()
    {
        cursor.enabled = false;
        eventSystem.SetSelectedGameObject(null);
        ServicesManager.HudManager.MessageBox.Show();
    }

    public void CloseInventory()
    {
        inventoryMenu.GetComponentInChildren<InventoryDrawer>().Close();
        inventoryMenu.gameObject.SetActive(false);
        cursor.enabled = true;
    }

    public Transform getActiveMenu()
    {
        if (moveMenu.elementGroup.enabled)
        {
            return moveMenu.transform;
        }
        if (contextMainMenu.enabled)
        {
            return contextMainMenu.transform;
        }
        return null;
    }
}
