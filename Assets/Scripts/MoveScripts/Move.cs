using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public MoveData moveData;
    private int pp;

    public void InitiateFromMoveData(MoveData moveData)
    {
        this.moveData = moveData;
        gameObject.name = moveData.MoveName;
        pp = moveData.MaxPP;
    }
}

