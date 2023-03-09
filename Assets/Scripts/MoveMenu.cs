﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MoveMenu: MonoBehaviour, IMenuInteractable
{
    public MenuItem moveMenuItemPrefab;
    public RectTransform moveList;

    [HideInInspector]
    public ElementGroup elementGroup;

    ActorController playerActorController;
    MoveRegistry moveRegistry;
    MoveMenuDetail moveMenuDetail;

    Dictionary<MenuItem, Move> itemToMoveMap = new Dictionary<MenuItem, Move>();

    public EventHandler<EventArgs> onChooseMove;

    public void Start()
    {
        this.elementGroup = GetComponent<ElementGroup>();
        moveMenuDetail = GetComponentInChildren<MoveMenuDetail>();
    }

    public void SetupMenu()
    {
        PlayerInputController playerInputController = FindObjectOfType<PlayerInputController>();
        playerActorController = playerInputController.GetComponent<ActorController>();

        itemToMoveMap.Clear();
        Array.ForEach(GetComponentsInChildren<MenuItem>(), (childMenuItem) =>
        {
            Destroy(childMenuItem.gameObject);
        });

        foreach (Move move in playerActorController.moves)
        {
            MenuItem menuItem = Instantiate(moveMenuItemPrefab, moveList);
            menuItem.GetComponent<MoveMenuSelection>().UpdateWithMoveData(move);
            itemToMoveMap[menuItem] = move;
            menuItem.AttachMenuListener(this);
            menuItem.onSelect += OnChooseMove;
        }
    }

    public void OnChooseMove(object sender, EventArgs args)
    {
        MenuItem menuItem = (MenuItem)sender;
        Move moveData = itemToMoveMap[menuItem];
        playerActorController.UseMove(moveData);
        Array.ForEach(GetComponentsInChildren<MenuItem>(), (childMenuItem) =>
        {
            childMenuItem.onSelect -= OnChooseMove;
        });
        elementGroup.Hide();

        if (onChooseMove != null)
        {
            onChooseMove(this, EventArgs.Empty);
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
}
