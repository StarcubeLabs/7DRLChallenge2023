using UnityEngine;

public class Move : MonoBehaviour
{
    public MoveData moveData;
    public int pp;

    public static Move InitiateFromMoveData(MoveData moveData)
    {
        GameObject moveObject = new GameObject();
        Move move = moveObject.AddComponent<Move>();
        move.moveData = moveData;
        moveObject.name = moveData.MoveName;
        move.pp = moveData.MaxPP;
        return move;
    }
}

