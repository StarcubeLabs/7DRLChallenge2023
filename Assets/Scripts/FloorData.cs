using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="FloorData", menuName="Starcube/7DRL/FloorData", order=2)]
[System.Serializable]
public class FloorData : ScriptableObject
{
    public Vector2Int widthRange = new Vector2Int(30, 30);
    public Vector2Int heightRange = new Vector2Int(20, 30);
    public Vector2Int roomNumRange = new Vector2Int(13, 13);
    public Vector2Int roomMaxSizeRange = new Vector2Int(5, 5);
    public Vector2Int roomMinSizeRange = new Vector2Int(3, 3);

    public enum FloorTypes
    {
        Normal,
        Grass,
        Fire,
        Water,
    };

    public FloorTypes CurrentFloorType;

    public void GenerateFloorEntities()
    {
        if (NumberOfEnemiesToSpawnRange.y < NumberOfEnemiesToSpawnRange.x)
        {
            Debug.LogError("Invalid range for Number of Enemies to Spawn, Y should be higher than X.");
        }
        
        if (NumberOfTrapsToSpawnRange.y < NumberOfTrapsToSpawnRange.x)
        {
            Debug.LogError("Invalid range for Number of Traps to Spawn, Y should be higher than X.");
        }

        if (NumberOfItemsToSpawn.y < NumberOfItemsToSpawn.x)
        {
            Debug.LogError("Invalid range for Number of Items to Spawn, Y should be higher than X.");
        }
        
        PotentialEnemies.ConstructWeightedTable(PotentialEnemyEntries);
        PotentialItems.ConstructWeightedTable(PotentialItemEntries);
        PotentialTraps.ConstructWeightedTable(PotentialTrapEntries);
    }

    public WeightedTable<ItemData> GetPotentialItems()
    {
        return PotentialItems;
    }

    public WeightedTable<Trap> GetPotentialTraps()
    {
        return PotentialTraps;
    }

    public WeightedTable<EnemyBaseScript> GetPotentialEnemies()
    {
        return PotentialEnemies;
    }

    /// <summary>
    /// This is the range of the amount of items to spawn.
    /// The X-Value is the minimum number of items to spawn.
    /// The Y-Value is the maximum number of items to spawn.
    /// </summary>
    [Tooltip("The range of items to spawn. X is the minimum [Inclusive], Y is the maximum [Inclusive].")]
    public Vector2Int NumberOfItemsToSpawn;
    public List<WeightedEntry<ItemData>> PotentialItemEntries = new List<WeightedEntry<ItemData>>();
    private WeightedTable<ItemData> PotentialItems = new WeightedTable<ItemData>();
    
    /// <summary>
    /// This is the range of the amount of traps to spawn.
    /// The X-Value is the minimum number of traps to spawn.
    /// The Y-Value is the maximum number of traps to spawn.
    /// </summary>
    [HideInInspector]
    [Tooltip("The range of traps to spawn. X is the minimum [Inclusive], Y is the maximum [Inclusive].")]
    public Vector2Int NumberOfTrapsToSpawnRange;
    [HideInInspector]
    public List<WeightedEntry<Trap>> PotentialTrapEntries = new List<WeightedEntry<Trap>>();
    private WeightedTable<Trap> PotentialTraps = new WeightedTable<Trap>();

    /// <summary>
    /// This is the range of the amount of enemies to spawn.
    /// The X-Value is the minimum number of enemies to spawn.
    /// The Y-Value is the maximum number of enemies to spawn.
    /// </summary>
    [Tooltip("The range of enemies to spawn. X is the minimum [Inclusive], Y is the maximum [Inclusive].")]
    public Vector2Int NumberOfEnemiesToSpawnRange;
    public List<WeightedEntry<EnemyBaseScript>> PotentialEnemyEntries = new List<WeightedEntry<EnemyBaseScript>>();
    private WeightedTable<EnemyBaseScript> PotentialEnemies = new WeightedTable<EnemyBaseScript>();
}
