using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    
    
    void Start()
    {
        gameObject.AddComponent<SpriteRenderer>();
        gameObject.GetComponent<SpriteRenderer>().sprite = TrapData.trapVisual_Temp;
    }
    
    public BasicTrap TrapData;

    public Vector3Int gridPosition;
    
    public void SnapToPosition(Vector3Int gridPosition)
    {
        var grid = FindObjectOfType<Grid>();

        //Disable pure location snap so "faux-animation" lerping can work instead.
        Vector3 worldPosition = grid.GetCellCenterWorld(gridPosition);
        this.transform.position = new Vector3(worldPosition.x, 0, worldPosition.z);
    }
    
    public void OnContact(ActorController collidedActor)
    {
        TrapData.ActivateTrap(collidedActor);
    }
}
