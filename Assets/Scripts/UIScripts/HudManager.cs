using System;
using System.Collections.Generic;
using System.Linq;
using RLDataTypes;
using TMPro;
using UnityEngine;

public class HudManager: MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI floorNumberText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI accessoryText;
    public TextMeshProUGUI statusText;

    public TextMeshProUGUI floorPromptText;
    private ContextMenu contextMenu;
    public ContextMenu ContextMenu { get { return contextMenu; } }
    private MessageBox messageBox;
    public MessageBox MessageBox { get { return messageBox; } }
    [SerializeField]
    private GameObject blindness;

    PlayerInputController playerInputController;
    LevelManager levelManager;
    Animator animator;

    public const int FLOOR_NOTIFICATION_ANIMATION_LAYER = 1;

    public void Start()
    {
        playerInputController = FindObjectOfType<PlayerInputController>();
        contextMenu = GetComponentInChildren<ContextMenu>();
        messageBox = GetComponentInChildren<MessageBox>();
        levelManager = FindObjectOfType<LevelManager>();
        animator = GetComponent<Animator>();

        levelManager.onLevelChange += OnEnterFloor;
    }

    public void Update()
    {
        healthText.text = string.Format("Health: {0}/{1}", playerInputController.playerActor.visualHitPoints, playerInputController.playerActor.hitPoints.y);
        hungerText.text = string.Format("Hunger: {0}", playerInputController.playerActor.visualHungerPoints);

        int floorNumber = levelManager.GetActiveLevel().transform.GetSiblingIndex() + 1;
        floorNumberText.text = string.Format("Floor: {0}", floorNumber);
        floorPromptText.text = string.Format("Floor {0}", floorNumber);

        string weapon = playerInputController.playerActor.Weapon ? playerInputController.playerActor.Weapon.ItemName : "Fists";
        string armor = playerInputController.playerActor.Armor ? playerInputController.playerActor.Armor.ItemName : "None";
        string accessory = playerInputController.playerActor.Accessory ? playerInputController.playerActor.Accessory.ItemName : "None";

        weaponText.text = string.Format("Weapon: {0} ", weapon);
        armorText.text = string.Format("Armor: {0} ", armor);
        accessoryText.text = string.Format("Accessory: {0} ", accessory);

        string statuses = "Healthy";
        if(playerInputController.playerActor.statuses.Count > 0)
        {
            statuses = "";
            foreach (Status status in playerInputController.playerActor.statuses)
            {
                statuses += status.HUD_TEXT + ",";
            }
            statuses = statuses.Substring(0, statuses.Length - 1);
        }
        statusText.text = string.Format("Status: {0}", statuses);


        blindness.SetActive(playerInputController.playerActor.HasVisualStatus(StatusType.Blindness));
    }

    public string MakeStatusHumanReadable<T>(string altText, string currentString, List<Status> statuses , ref int currentStatusCount) where T: Status
    {
        if (statuses.OfType<T>().Any())
        {
            currentStatusCount++;
            currentString += altText;
            if (currentStatusCount < statuses.Count)
            {
                currentString += ",";
            }
        }
        return currentString;
    }

    public void OnEnterFloor(object sender, EventArgs args)
    {
        animator.Play("appearing", FLOOR_NOTIFICATION_ANIMATION_LAYER);
    }
}
