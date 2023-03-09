using UnityEngine;
using System.Collections.Generic;

public class MoveRegistry : MonoBehaviour
{
    [SerializeField]
    public List<MoveData> moves { get; set; }

    [SerializeField]
    private MoveData basicAttack;
    private Move basicAttackMove;
    public Move BasicAttack { get { return basicAttackMove; } }

    public void Start()
    {
        basicAttackMove = Move.InitiateFromMoveData(basicAttack);
    }
}

