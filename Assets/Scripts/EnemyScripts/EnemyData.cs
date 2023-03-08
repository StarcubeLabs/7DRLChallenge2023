using System.Collections.Generic;
using RLDataTypes;
using UnityEngine;

[CreateAssetMenu(fileName="EnemyData", menuName="Starcube/7DRL/EnemyData", order=1)]
public class EnemyData : ScriptableObject
{
    public string enemyName = "Enemy";
    public GameObject enemyPrefab;

    [Header("Statistics")]
    [SerializeField]
    private int hitPointsMaximum;
    [HideInInspector]
    public Vector2Int hitPoints { get { return new Vector2Int(hitPointsMaximum, hitPointsMaximum); } }

    [Tooltip("Amount of damage the actor will deal without weapons.")]
    public int baseAttackPower;

    public ElementType elementType;
    public List<Move> startingMoves = new List<Move>();

    [Header("Equippables")]
    public Item weapon;
    public Item armor;
    public Item accessory;
}
