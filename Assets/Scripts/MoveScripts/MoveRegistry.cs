using UnityEngine;
using System.Collections.Generic;

public class MoveRegistry : MonoBehaviour
{
    [SerializeField]
    private List<MoveData> moves;

    [SerializeField]
    private MoveData basicAttack;
    private Move basicAttackMove;
    public Move BasicAttack { get { return basicAttackMove; } }

    public void Start()
    {
        basicAttackMove = Move.InitiateFromMoveData(basicAttack);
    }
}

