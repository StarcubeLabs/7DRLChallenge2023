using UnityEngine;

public class MoveRegistry : MonoBehaviour
{
    [SerializeField]
    private MoveData basicAttack;
    private Move basicAttackMove;
    public Move BasicAttack { get { return basicAttackMove; } }

    public void Start()
    {
        basicAttackMove = Move.InitiateFromMoveData(basicAttack);
    }
}

