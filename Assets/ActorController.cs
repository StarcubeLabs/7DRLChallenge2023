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
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        testMap = FindObjectOfType<TestMap>();

        gridPosition = grid.WorldToCell(this.transform.position);
        SnapToPosition(gridPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            Move(new Vector3Int(1, 0));
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Move(new Vector3Int(-1, 0));
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Move(new Vector3Int(0, -1));
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Move(new Vector3Int(0, 1));
        }
    }

    void Move(Vector3Int offset)
    {
        if (testMap.canWalkOnCell(gridPosition + offset))
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
}
