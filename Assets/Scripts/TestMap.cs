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

    public List<ActorController> MapEntityList;


    public EnemyBaseScript[] EnemyChoiceList;

    /// <summary>
    /// This is the range of the amount of enemies to spawn.
    /// The X-Value is the minimum number of enemies to spawn.
    /// The Y-Value is the maximum number of enemies to spawn.
    /// </summary>
    [Tooltip("The range of enemies to spawn. X is the minimum [Inclusive], Y is the maximum [Inclusive].")]
    public Vector2Int EnemySpawnRange;
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

        EnemiesForMap(cells);


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
        print("Can Walk on Cell: "+(somewhatInterestingMap.GetCell(position.x, somewhatInterestingMap.Height - 1 - position.y).IsWalkable && doesActorInhabitPosition(position) == false));
        return somewhatInterestingMap.GetCell(position.x, somewhatInterestingMap.Height - 1 - position.y).IsWalkable && doesActorInhabitPosition(position) == false;
    }

    public bool doesActorInhabitPosition(Vector3Int position)
    {
        for(int i = 0; i < MapEntityList.Count; i++)
        {
            if (MapEntityList[i].gridPosition == position)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Spawns Enemies for map.
    ///  How many enemies to spawn is exposed in the Inspector as 'EnemySpawnRange'.
    /// </summary>
    /// <param name="cells">The cells available to spawn on.</param>
    public void SpawnEnemiesForMap(Cell[] cells)
    {
        int NumberOfEnemiesToSpawn = Random.Range(EnemySpawnRange.x, EnemySpawnRange.y + 1);
        if (EnemySpawnRange.y > cells.Length)
        {
            NumberOfEnemiesToSpawn = cells.Length / 2;
            Debug.LogWarning("Number of Enemies to spawn exceeds cell count. Consider lowering the number.");
        }
        Debug.Log("Enemies Spawned:" + NumberOfEnemiesToSpawn);
        
        for (int numberOfSpawnedEnemies = 0; numberOfSpawnedEnemies < NumberOfEnemiesToSpawn; ++numberOfSpawnedEnemies)
        {
            EnemyBaseScript randomEnemy = Instantiate<EnemyBaseScript>(EnemyChoiceList[0]);

            //Set the enemy location.
            Vector3Int enemyGridPosition = getGridPositionFromCell(cells[Random.Range(0, cells.Length)]);
            randomEnemy.enemyActor.SnapToPosition(enemyGridPosition);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //print(canWalkOnCell(MapEntityList[1].gridPosition));
    }
}
