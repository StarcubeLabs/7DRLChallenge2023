using TMPro;
using UnityEngine;

public class HudManager: MonoBehaviour
{
    public TextMeshProUGUI healthText;
    private ContextMenu contextMenu;
    public ContextMenu ContextMenu { get { return contextMenu; } }
    private MessageBox messageBox;
    public MessageBox MessageBox { get { return messageBox; } }

    PlayerInputController playerInputController;
        

    public void Start()
    {
        playerInputController = FindObjectOfType<PlayerInputController>();
        contextMenu = GetComponentInChildren<ContextMenu>();
        messageBox = GetComponentInChildren<MessageBox>();
    }

    public void Update()
    {
        healthText.text = string.Format("Health: {0}/{1}", playerInputController.playerActor.visualHitPoints, playerInputController.playerActor.hitPoints.y);
    }
}
