using RogueSharp;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    Grid grid;
    Vector3Int gridPosition;
    TestMap testMap;

    ServicesManager servicesManager;
    public Transform cameraSlot;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        testMap = FindObjectOfType<TestMap>();
        servicesManager = FindObjectOfType<ServicesManager>();
        servicesManager.camera.transform.SetParent(cameraSlot, false);
        servicesManager.camera.transform.localRotation = Quaternion.identity;
        servicesManager.camera.transform.localPosition = Vector3.zero;

        gridPosition = grid.WorldToCell(this.transform.position);
        SnapToPosition(gridPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3Int offset)
    {
        if (testMap.canWalkOnCell(gridPosition + offset))
        {
            gridPosition += offset;
            SnapToPosition(gridPosition);
        }
    }

    public void MoveDiagonal(Vector3Int offset)
    {
        if ( testMap.canWalkOnCell(gridPosition + offset) && testMap.canWalkOnCell(gridPosition + new Vector3Int(offset.x, 0, 0)) && testMap.canWalkOnCell(gridPosition + new Vector3Int(0, offset.y, 0)))
        {
            gridPosition += offset;
            SnapToPosition(gridPosition);
        }
    }

    public void SnapToPosition(Vector3Int gridPosition)
    {
        grid = FindObjectOfType<Grid>();
        Vector3 worldPosition = grid.GetCellCenterWorld(gridPosition);
        this.transform.position = new Vector3(worldPosition.x, 0, worldPosition.z);
    }

    public void MoveToward(Vector3Int gridPosition)
    {

    }
}
