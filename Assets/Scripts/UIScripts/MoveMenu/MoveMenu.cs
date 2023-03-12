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
    public float menuCloseDelayTime = 1.5f;

    private MoveMenuMode mode;
    private Move lastSelectedMove;
    private float selectionTimeStamp;

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
        RectTransform rect = (RectTransform)this.transform;
        if (mode is MoveMenuTeachMove)
        {
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300);
        }
        else
        {
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 240);
        }
        foreach (Move move in mode.GetMoves())
        {
            MenuItem menuItem = Instantiate(moveMenuItemPrefab, moveList);
            menuItem.GetComponent<MoveMenuSelection>().UpdateWithMoveData(move);
            itemToMoveMap[menuItem] = move;
            menuItem.AttachMenuListener(this);
            menuItem.AttachMenuListener(contextMenu);
            if (mode.IsMoveSelectable(move))
            {
                menuItem.onSelect += OnStartChooseMove;
            }
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(moveList);

        elementGroup.Show();
    }

    public void OnStartChooseMove(object sender, EventArgs args)
    {
        MenuItem menuItem = (MenuItem)sender;
        lastSelectedMove = itemToMoveMap[menuItem];
        Array.ForEach(GetComponentsInChildren<MenuItem>(), (childMenuItem) =>
        {
            childMenuItem.onSelect -= OnStartChooseMove;
        });
        elementGroup.Hide();
        Close();

        bool confirmed = mode.ConfirmChooseMove(lastSelectedMove);
        if (confirmed)
        {
            if (onChooseMove != null && mode.ConfirmChooseMove(lastSelectedMove))
            {
                onChooseMove(this, EventArgs.Empty);
            }
            selectionTimeStamp = Time.time;
        }

        if (!confirmed)
        {
            mode.OnCancel();
        }
    }

    public void Update()
    {
        if (lastSelectedMove && (Time.time - selectionTimeStamp) > menuCloseDelayTime)
        {
            OnFinishMove();
            lastSelectedMove = null;
        }
    }

    public void OnFinishMove()
    {
        mode.SelectMove(lastSelectedMove);
        mode.OnClose();
    }

    public void StartHighlightMenuItem(MenuItem menuItem)
    {
        Move moveData = itemToMoveMap[menuItem];
        moveMenuDetail.UpdateWithMoveData(moveData);
    }

    public void StopHighlightMenuItem(MenuItem menuItem)
    {
    }

    public bool IsOpen()
    {
        return elementGroup.enabled || lastSelectedMove;
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
    }
}
