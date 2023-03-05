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
    public FieldOfView fieldOfView;

    public EntityManager entityManager;
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
        entityManager = FindObjectOfType<EntityManager>();
        SetupTileMap();
        SpawnEntities();

        //Path Finder Setup
        pathFinder = new PathFinder(somewhatInterestingMap, float.PositiveInfinity - 1);
        fieldOfView = new FieldOfView(somewhatInterestingMap);
    }

    public void SetupTileMap()
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
        
        tileMap.SetTile(GetGridPositionFromCell(somewhatInterestingMap.start), stairDown);
        tileMap.SetTile(GetGridPositionFromCell(somewhatInterestingMap.end), stairUp);
    }

    public void SpawnEntities()
    {
        ActorController actorController = FindObjectOfType<ActorController>();
        Cell[] cells = somewhatInterestingMap.GetAllCells().Where(cell => cell.IsWalkable).ToArray();
        int randomWalkableCellIndex = Random.Range(0, cells.Length);
        //Set the player location.
        Vector3Int gridPosition = GetGridPositionFromCell(somewhatInterestingMap.start);
        actorController.SnapToPosition(gridPosition);

        SpawnEnemiesForMap(cells);
    }

    public Vector3Int GetGridPositionFromCell(Cell cell)
    {
        return new Vector3Int(cell.X, somewhatInterestingMap.Height - 1 - cell.Y, 0);
    }

    public Vector3Int GetGridPositionFromCell(ICell cell)
    {
        return new Vector3Int(cell.X, somewhatInterestingMap.Height - 1 - cell.Y, 0);
    }

    public Cell GetCellFromGridPosition(Vector3Int pos)
    {
        return somewhatInterestingMap.GetCell(pos.x, somewhatInterestingMap.Height - 1 - pos.y);
    }

    public bool CanWalkOnCell(Vector3Int position)
    {
        //print("Can Walk on Cell: "+(somewhatInterestingMap.GetCell(position.x, somewhatInterestingMap.Height - 1 - position.y).IsWalkable && !entityManager.isEntityInPosition(position)));
        return somewhatInterestingMap.GetCell(position.x, somewhatInterestingMap.Height - 1 - position.y).IsWalkable && !entityManager.isEntityInPosition(position);
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
            Vector3Int enemyGridPosition = GetGridPositionFromCell(cells[Random.Range(0, cells.Length)]);
            randomEnemy.enemyActor.SnapToPosition(enemyGridPosition);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //print(canWalkOnCell(MapEntityList[1].gridPosition));
    }

    public bool CanSeePosition(Vector3Int position, Vector3Int target)
    {
        ICell positionCell = GetCellFromGridPosition(position);
        ICell targetCell = GetCellFromGridPosition(target);

        fieldOfView.ComputeFov(positionCell.X, positionCell.Y, 30, false);
        return fieldOfView.IsInFov(targetCell.X, targetCell.Y);
    }
}
