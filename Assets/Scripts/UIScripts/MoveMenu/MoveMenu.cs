using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveMenu: MonoBehaviour, IMenuInteractable
{
    public MenuItem moveMenuItemPrefab;
    public RectTransform moveList;

    [HideInInspector]
    public ElementGroup elementGroup;

    ActorController playerActorController;
    public ActorController PlayerActorController { get { return playerActorController; } }
    MoveRegistry moveRegistry;
    MoveMenuDetail moveMenuDetail;

    Dictionary<MenuItem, Move> itemToMoveMap = new Dictionary<MenuItem, Move>();
    public List<MenuItem> MenuItems { get { return new List<MenuItem>(itemToMoveMap.Keys); }}

    public EventHandler<EventArgs> onChooseMove;

    private MoveMenuMode mode;

    public void Start()
    {
        this.elementGroup = GetComponent<ElementGroup>();
        moveMenuDetail = GetComponentInChildren<MoveMenuDetail>();
        PlayerInputController playerInputController = FindObjectOfType<PlayerInputController>();
        playerActorController = playerInputController.GetComponent<ActorController>();
    }

    public void SetupUseMoveMenu(ContextMenu contextMenu)
    {
        mode = new MoveMenuUseMove(this);
        SetupMenu(contextMenu);
    }

    public void SetupTeachMoveMenu(ContextMenu contextMenu, Move newMove, Action consumeAction)
    {
        mode = new MoveMenuTeachMove(this, newMove, consumeAction);
        SetupMenu(contextMenu);
    }

    private void SetupMenu(ContextMenu contextMenu)
    {
        foreach (Move move in mode.GetMoves())
        {
            MenuItem menuItem = Instantiate(moveMenuItemPrefab, moveList);
            menuItem.GetComponent<MoveMenuSelection>().UpdateWithMoveData(move);
            itemToMoveMap[menuItem] = move;
            menuItem.AttachMenuListener(this);
            menuItem.AttachMenuListener(contextMenu);
            if (mode.IsMoveSelectable(move))
            {
                menuItem.onSelect += OnChooseMove;
            }
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(moveList);

        elementGroup.Show();
    }

    public void OnChooseMove(object sender, EventArgs args)
    {
        MenuItem menuItem = (MenuItem)sender;
        Move moveData = itemToMoveMap[menuItem];
        mode.SelectMove(moveData);
        Array.ForEach(GetComponentsInChildren<MenuItem>(), (childMenuItem) =>
        {
            childMenuItem.onSelect -= OnChooseMove;
        });
        elementGroup.Hide();

        bool confirmed = mode.ConfirmChooseMove(moveData);
        if (confirmed)
        {
            if (onChooseMove != null && mode.ConfirmChooseMove(moveData))
            {
                onChooseMove(this, EventArgs.Empty);
            }
        }

        Close();

        if (!confirmed)
        {
            mode.OnCancel();
        }
    }

    public void StartHighlightMenuItem(MenuItem menuItem)
    {
        Move moveData = itemToMoveMap[menuItem];
        moveMenuDetail.UpdateWithMoveData(moveData);
    }

    public void StopHighlightMenuItem(MenuItem menuItem)
    {
    }

    public void Cancel()
    {
        mode.OnCancel();
    }

    public void Close()
    {
        itemToMoveMap.Clear();
        Array.ForEach(GetComponentsInChildren<MenuItem>(), childMenuItem =>
        {
            Destroy(childMenuItem.gameObject);
        });
        
        elementGroup.Hide();
        mode.OnClose();
    }
}
