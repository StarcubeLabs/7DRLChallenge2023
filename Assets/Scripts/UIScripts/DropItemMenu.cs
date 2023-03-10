using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemMenu : MonoBehaviour
{
    public DropMenu_CurrentItemDisplay CurrentItemDisplay;
    public DropMenu_DropItem CurrentDropItemDisplay;

    public void Close()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        ServicesManager.HudManager.ContextMenu.Close();
    }
    
}
