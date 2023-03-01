using RogueSharp;
using RogueSharp.MapCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TestMap : MonoBehaviour
{
    public Tilemap tileMap;
    public Tile wall;
    public Tile land;
    public IMap somewhatInterestingMap;
    // Start is called before the first frame update
    void Start()
    {
        tileMap = GetComponent<Tilemap>();

        tileMap.SetTile(new Vector3Int(0, 0, 0), land);

        IMapCreationStrategy<Map> mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(30, 20, 8, 5, 3);
        somewhatInterestingMap = Map.Create(mapCreationStrategy);
        Debug.Log(somewhatInterestingMap.ToString());

        for (int i = 0; i < somewhatInterestingMap.Width; i++)
        {
            for (int j = 0; j < somewhatInterestingMap.Height; j++)
            {
                Cell cell = somewhatInterestingMap.GetCell(i, somewhatInterestingMap.Height - 1 - j);
                TileBase tileBase = wall;
                if (cell.IsWalkable)
                {
                    tileBase = land;
                }
                tileMap.SetTile(new Vector3Int(i, j, 0), tileBase);
            }
        }

        ActorController actorController = FindObjectOfType<ActorController>();
        Cell[] cells = somewhatInterestingMap.GetAllCells().Where(cell => cell.IsWalkable).ToArray();
        int randomWalkableCellIndex = Random.Range(0, cells.Length);

        Vector3Int gridPosition = getGridPositionFromCell(cells[randomWalkableCellIndex]);
        actorController.SnapToPosition(gridPosition);
    }

    public Vector3Int getGridPositionFromCell(Cell cell)
    {
        return new Vector3Int(cell.X, somewhatInterestingMap.Height - 1 - cell.Y, 0);
    }

    public bool canWalkOnCell(Vector3Int position)
    {
        return somewhatInterestingMap.GetCell(position.x, somewhatInterestingMap.Height - 1 - position.y).IsWalkable;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
