using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContextMenu: MonoBehaviour
{
    public EventSystem eventSystem;
    public Image cursor;

    public Transform contextMainMenu;

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

        eventSystem.firstSelectedGameObject = moveMenuItem.gameObject;

        moveMenuItem.onSelect += OnSelectMoveMenu;
    }

    public void DeselectMenuItem(MenuItem sender)
    {
        if (cursor.transform.parent == sender.transform)
        {
            cursor.transform.SetParent(null);
            cursor.enabled = false;
        }
    }

    public void SelectMenuItem(MenuItem sender)
    {
        cursor.rectTransform.SetParent(sender.transform, false);
        cursor.rectTransform.localPosition = new Vector2(-20 + ((RectTransform)(sender.transform)).rect.xMin, 0);
        cursor.enabled = true;
    }

    public void OnSelectMoveMenu(object sender, EventArgs eventArgs)
    {
        moveMenu.gameObject.SetActive(true);
        contextMainMenu.gameObject.SetActive(false);

        SelectFirstMenuItem();
    }

    public void OpenMenu()
    {
        contextMainMenu.gameObject.SetActive(true);
    }

    public void SelectFirstMenuItem()
    {
        Transform root = getActiveMenu();
        GameObject menuItem = root.GetComponentInChildren<MenuItem>(true).gameObject;
        eventSystem.firstSelectedGameObject = menuItem;
        eventSystem.SetSelectedGameObject(menuItem);
    }

    public void NavigateDown()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {

            SelectFirstMenuItem();
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
            SelectFirstMenuItem();
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
        if (moveMenu.gameObject.activeSelf)
        {
            moveMenu.gameObject.SetActive(false);
            contextMainMenu.gameObject.SetActive(true);
            SelectFirstMenuItem();
        }
        else if (contextMainMenu.gameObject.activeSelf)
        {
            contextMainMenu.gameObject.SetActive(false);
        }
    }

    public bool IsMenuOpen()
    {
        return moveMenu.gameObject.activeSelf || contextMainMenu.gameObject.activeSelf;
    }

    public Transform getActiveMenu()
    {
        if (moveMenu.gameObject.activeSelf)
        {
            return moveMenu.transform;
        }
        if (contextMainMenu.gameObject.activeSelf)
        {
            return contextMainMenu.transform;
        }
        return null;
    }
}
