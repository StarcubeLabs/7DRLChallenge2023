using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

class HudManager: MonoBehaviour
{
    public TextMeshProUGUI healthText;

    PlayerInputController playerInputController;
        

    public void Start()
    {
        playerInputController = FindObjectOfType<PlayerInputController>();
    }

    public void Update()
    {
        healthText.text = string.Format("Health: {0}/{1}", playerInputController.playerActor.hitPoints.x, playerInputController.playerActor.hitPoints.y);
    }
}
