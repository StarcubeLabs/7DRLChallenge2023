using RogueSharp;
using RogueSharp.MapCreation;
using RogueSharp.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class LevelController : MonoBehaviour
{
    public Tilemap tileMap;
    public GameObject wall;
    public Tile land;
    public Tile stairDown;
    public Tile stairUp;
    public DungeonMap somewhatInterestingMap;
    public PathFinder pathFinder;
    public FieldOfView fieldOfView;

    public int randomSeed;

    public EntityManager entityManager;
    public Item itemPrefab;

    public EnemyBaseScript[] EnemyChoiceList;

    public List<EntityController> entitiesOnLevel = new List<EntityController>();

    /// <summary>
    /// This is the range of the amount of items to spawn.
    /// The X-Value is the minimum number of items to spawn.
    /// The Y-Value is the maximum number of items to spawn.
    /// </summary>
    [Tooltip("The range of items to spawn. X is the minimum [Inclusive], Y is the maximum [Inclusive].")]
    public Vector2Int ItemSpawnRange;
    public List<WeightedEntry<ItemData>> PotentialItemEntries = new List<WeightedEntry<ItemData>>();
    private WeightedTable<ItemData> PotentialItems = new WeightedTable<ItemData>();
    
    /// <summary>
    /// This is the range of the amount of traps to spawn.
    /// The X-Value is the minimum number of traps to spawn.
    /// The Y-Value is the maximum number of traps to spawn.
    /// </summary>
    [Tooltip("The range of traps to spawn. X is the minimum [Inclusive], Y is the maximum [Inclusive].")]
    public Vector2Int NumberOfTrapsToSpawnRange;
    public List<WeightedEntry<Trap>> PotentialTrapEntries = new List<WeightedEntry<Trap>>();
    private WeightedTable<Trap> PotentialTraps = new WeightedTable<Trap>();
    
    /// <summary>
    /// This is the range of the amount of enemies to spawn.
    /// The X-Value is the minimum number of enemies to spawn.
    /// The Y-Value is the maximum number of enemies to spawn.
    /// </summary>
    [Tooltip("The range of enemies to spawn. X is the minimum [Inclusive], Y is the maximum [Inclusive].")]
    public Vector2Int EnemySpawnRange;

    // Start is called before the first frame update
    public void Initialize()
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

        DotNetRandom random = randomSeed == 0 ? Singleton.DefaultRandom : new DotNetRandom(randomSeed);
        IMapCreationStrategy<DungeonMap> mapCreationStrategy = new RandomDungeonRoomsMapCreationStrategy(30, 20, 13, 5, 3, random);
        somewhatInterestingMap = Map.Create<DungeonMap>(mapCreationStrategy);
        //Debug.Log(somewhatInterestingMap.ToString());

        for (int i = 0; i < somewhatInterestingMap.Width; i++)
        {
            for (int j = 0; j < somewhatInterestingMap.Height; j++)
            {
                Cell cell = somewhatInterestingMap.GetCell(i, somewhatInterestingMap.Height - 1 - j);
                if (cell.IsWalkable)
                {
                    tileMap.SetTile(new Vector3Int(i, j, 0), land);
                }
                else
                {
                    Instantiate(wall, new Vector3(i, 0, j), Quaternion.identity, transform);
                }
            }
        }
        
        tileMap.SetTile(GetGridPositionFromCell(somewhatInterestingMap.start), stairDown);
        tileMap.SetTile(GetGridPositionFromCell(somewhatInterestingMap.end), stairUp);
    }

    public void SetupPlayer()
    {
        PlayerInputController playerController = FindObjectOfType<PlayerInputController>();
        Vector3Int gridPosition = GetGridPositionFromCell(somewhatInterestingMap.start);
        playerController.playerActor.SnapToPosition(gridPosition);
    }

    public void SpawnEntities()
    {
        if (randomSeed != 0)
        {
            Random.InitState(randomSeed);
        }

        Cell[] cells = somewhatInterestingMap.GetAllCells().Where(cell => cell.IsWalkable).ToArray();

        PotentialItems.ConstructWeightedTable(PotentialItemEntries);
        PotentialTraps.ConstructWeightedTable(PotentialTrapEntries);
        
        SpawnEnemiesForMap(cells);
        SpawnItemsForMap(cells);
        SpawnTrapsForMap(cells);
    }

    public void AddEntityToLevel(EntityController entityController)
    {
        if (!entitiesOnLevel.Contains(entityController))
        {
            entitiesOnLevel.Add(entityController);
        }
    }

    public void RemoveEntityFromLevel(EntityController entityController)
    {
        entitiesOnLevel.Remove(entityController);
    }

    public void SaveEntities()
    {
        foreach(EntityController entity in entitiesOnLevel)
        {
            entity.transform.SetParent(this.transform);
            entity.SaveEntity(entityManager);
        }
    }

    public void LoadEntities()
    {
        foreach (EntityController entity in entitiesOnLevel)
        {
            entity.transform.SetParent(null);
            entity.LoadEntity(entityManager);
        }
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
        return IsCellWalkable(position) && !entityManager.isEntityInPosition(position);
    }

    public bool IsCellWalkable(Vector3Int position)
    {
        return somewhatInterestingMap.GetCell(position.x, somewhatInterestingMap.Height - 1 - position.y).IsWalkable;
    }

    public bool HasInteractable(Vector3Int position)
    {
        return entityManager.isInteractableInPosition(position);
    }
    
    public bool HasTrap(Vector3Int position)
    {
        return entityManager.isTrapInPosition(position);
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
        //Debug.LogFormat("Enemies Spawned: {0}", NumberOfEnemiesToSpawn);
        
        for (int numberOfSpawnedEnemies = 0; numberOfSpawnedEnemies < NumberOfEnemiesToSpawn; ++numberOfSpawnedEnemies)
        {
            EnemyBaseScript randomEnemy = Instantiate<EnemyBaseScript>(EnemyChoiceList[Random.Range(0, EnemyChoiceList.Length)]);

            //Set the enemy location.
            Vector3Int enemyGridPosition = GetGridPositionFromCell(cells[Random.Range(0, cells.Length)]);
            //Debug.LogFormat("Level {0} Spawned at position {1}", this.transform.GetSiblingIndex(), enemyGridPosition.ToString());
            randomEnemy.enemyActor.SnapToPosition(enemyGridPosition);
            randomEnemy.transform.SetParent(this.transform);
            AddEntityToLevel(randomEnemy.enemyActor);
        }
    }
    
    /// <summary>
    /// Spawns Enemies for map.
    ///  How many enemies to spawn is exposed in the Inspector as 'EnemySpawnRange'.
    /// </summary>
    /// <param name="cells">The cells available to spawn on.</param>
    public void SpawnItemsForMap(Cell[] cells)
    {
        int numberOfItemsToSpawn = Random.Range(ItemSpawnRange.x, ItemSpawnRange.y + 1);

        if (ItemSpawnRange.y > cells.Length)
        {
            numberOfItemsToSpawn = cells.Length / 2;
            Debug.LogWarning("Number of Items to spawn exceeds cell count. Consider lowering the number.");
        }
        //Debug.Log("Items Spawned:" + numberOfItemsToSpawn);
        
        for (int numberOfSpawnedItems = 0; numberOfSpawnedItems < numberOfItemsToSpawn; ++numberOfSpawnedItems)
        {
            Item randomItem = Instantiate<Item>(itemPrefab);
            randomItem.ItemData = PotentialItems.GetRandomEntry();

            randomItem.transform.Rotate(new Vector3(90,0,0));
            Instantiate(randomItem.ItemObject, randomItem.transform);
            //Set the enemy location.
            Vector3Int itemGridPosition = GetGridPositionFromCell(cells[Random.Range(0, cells.Length)]);
            randomItem.GenerateRandomStackSize();
            randomItem.SnapToPosition(itemGridPosition);
            randomItem.gridPosition = itemGridPosition;
            randomItem.GetComponent<SpriteRenderer>().sortingOrder = 1;
            randomItem.transform.SetParent(this.transform);
            AddEntityToLevel(randomItem);
        }
    }
    
    /// <summary>
    /// Spawns Enemies for map.
    ///  How many enemies to spawn is exposed in the Inspector as 'EnemySpawnRange'.
    /// </summary>
    /// <param name="cells">The cells available to spawn on.</param>
    public void SpawnTrapsForMap(Cell[] cells)
    {
        int numberOfTrapsToSpawn = Random.Range(NumberOfTrapsToSpawnRange.x, NumberOfTrapsToSpawnRange.y + 1);

        if (ItemSpawnRange.y > cells.Length)
        {
            numberOfTrapsToSpawn = cells.Length / 2;
            Debug.LogWarning("Number of Traps to spawn exceeds cell count. Consider lowering the number.");
        }
        //Debug.Log("Traps Spawned:" + numberOfTrapsToSpawn);
        
        for (int numberOfSpawnedTraps = 0; numberOfSpawnedTraps < numberOfTrapsToSpawn; ++numberOfSpawnedTraps)
        {
            Trap randomTrap = Instantiate<Trap>(PotentialTraps.GetRandomEntry());
            
            randomTrap.transform.Rotate(new Vector3(90,0,0));
            //Set the enemy location.
            Vector3Int trapGridPosition = GetGridPositionFromCell(cells[Random.Range(0, cells.Length)]);
            randomTrap.SnapToPosition(trapGridPosition);
            randomTrap.gridPosition = trapGridPosition;
            randomTrap.GetComponent<SpriteRenderer>().sortingOrder = 1;
            entityManager.AddTrap(randomTrap);
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
