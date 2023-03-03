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
    public Tile stairDown;
    public Tile stairUp;
    public DungeonMap somewhatInterestingMap;
    public PathFinder pathFinder;

    public EnemyBaseScript[] EnemyChoiceList;
    // Start is called before the first frame update
    void Start()
    {
        tileMap = GetComponent<Tilemap>();

        tileMap.SetTile(new Vector3Int(0, 0, 0), land);

        IMapCreationStrategy<DungeonMap> mapCreationStrategy = new RandomDungeonRoomsMapCreationStrategy(30, 20, Random.Range(3,13), 5, 3);
        somewhatInterestingMap = Map.Create<DungeonMap>(mapCreationStrategy);
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
        
        tileMap.SetTile(getGridPositionFromCell(somewhatInterestingMap.start), stairDown);
        tileMap.SetTile(getGridPositionFromCell(somewhatInterestingMap.end), stairUp);

        ActorController actorController = FindObjectOfType<ActorController>();
        Cell[] cells = somewhatInterestingMap.GetAllCells().Where(cell => cell.IsWalkable).ToArray();
        int randomWalkableCellIndex = Random.Range(0, cells.Length);
        //Set the player location.
        Vector3Int gridPosition = getGridPositionFromCell(somewhatInterestingMap.start);
        actorController.SnapToPosition(gridPosition);

        //Make an Enemy
        EnemyBaseScript randomEnemy = Instantiate<EnemyBaseScript>(EnemyChoiceList[0]);

        //Set the enemy location.
        Vector3Int enemyGridPosition = getGridPositionFromCell(cells[Random.Range(0, cells.Length)]);
        randomEnemy.enemyActor.SnapToPosition(enemyGridPosition);

        //Path Finder Setup
        pathFinder = new PathFinder(somewhatInterestingMap, 1.0);
    }

    public Vector3Int getGridPositionFromCell(Cell cell)
    {
        return new Vector3Int(cell.X, somewhatInterestingMap.Height - 1 - cell.Y, 0);
    }

    public Vector3Int getGridPositionFromCell(ICell cell)
    {
        return new Vector3Int(cell.X, somewhatInterestingMap.Height - 1 - cell.Y, 0);
    }

    public Cell getCellFromGridPosition(Vector3Int pos)
    {
        return somewhatInterestingMap.GetCell(pos.x, pos.y);
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
