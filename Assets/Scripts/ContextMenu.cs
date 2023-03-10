using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        eventSystem.SetSelectedGameObject(null);
        cursor.enabled = false;
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
        moveMenu.Show();
        contextMainMenu.Hide();

        NavigateToFirstMenuItem();
    }

    public void OnSelectInventory(object sender, EventArgs eventArgs)
    {
        inventoryMenu.gameObject.SetActive(true);
        inventoryMenu.GetComponentInChildren<InventoryDrawer>().Open();
        contextMainMenu.Hide();
        cursor.enabled = false;
    }

    public void OpenMenu()
    {
        contextMainMenu.Show();

        moveMenu.SetupMenu();

        Array.ForEach(moveMenu.GetComponentsInChildren<MenuItem>(), (menuItem) =>
        {
            menuItem.AttachMenuListener(this);
        });

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
            moveMenu.elementGroup.Hide();
            contextMainMenu.Show();
            NavigateToFirstMenuItem();
        }
        else if (inventoryMenu.gameObject.activeSelf)
        {
            inventoryMenu.GetComponentInChildren<InventoryDrawer>().Close();
            inventoryMenu.gameObject.SetActive(false);
            contextMainMenu.Show();
            NavigateToFirstMenuItem();
            cursor.enabled = true;
        }
        else if (contextMainMenu.enabled)
        {
            cursor.enabled = false;
            contextMainMenu.Hide();
            eventSystem.SetSelectedGameObject(null);
        }
    }

    public bool IsMenuOpen()
    {
        return moveMenu.elementGroup.enabled || contextMainMenu.enabled || inventoryMenu.gameObject.activeSelf;
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
