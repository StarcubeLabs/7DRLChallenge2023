using System;
using RLDataTypes;
using TMPro;
using UnityEngine;

public class HudManager: MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI floorNumberText;
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
        
        blindness.SetActive(playerInputController.playerActor.HasVisualStatus(StatusType.Blindness));
    }

    public void OnEnterFloor(object sender, EventArgs args)
    {
        animator.Play("appearing", FLOOR_NOTIFICATION_ANIMATION_LAYER);
    }
}
