using RogueSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    Grid grid;
    public Vector3Int gridPosition;
    TestMap testMap;
    
    GameStateManager gameStateManager;
    EntityManager entityManager;
    TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        testMap = FindObjectOfType<TestMap>();
        gameStateManager = FindObjectOfType<GameStateManager>();
        turnManager = FindObjectOfType<TurnManager>();
        entityManager = FindObjectOfType<EntityManager>();
        entityManager.AddActor(this);

        gridPosition = grid.WorldToCell(this.transform.position);
        SnapToPosition(gridPosition);
    }

    private void OnDestroy()
    {
        entityManager.RemoveActor(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3Int offset)
    {
        if (!turnManager.CanMove(this))
        {
            return;
        }
        if (testMap.CanWalkOnCell(gridPosition + offset))
        {
            
            gridPosition += offset;
            SnapToPosition(gridPosition);
            turnManager.KickToBackOfTurnOrder(this);
        }
    }

    /// <summary>
    /// Moves an Actor while checking diagonals to prevent corner cutting of solid or filled in grid squares.
    /// </summary>
    /// <param name="offset">The direction of the square adjacent to the Actor to move in.</param>
    public void MoveDiagonal(Vector3Int offset)
    {
        if (!turnManager.CanMove(this))
        {
            return;
        }
        if ( testMap.CanWalkOnCell(gridPosition + offset) && testMap.CanWalkOnCell(gridPosition + new Vector3Int(offset.x, 0, 0)) && testMap.CanWalkOnCell(gridPosition + new Vector3Int(0, offset.y, 0)))
        {
            
            gridPosition += offset;
            SnapToPosition(gridPosition);
        }
        turnManager.KickToBackOfTurnOrder(this);
    }

    public void SnapToPosition(Vector3Int gridPosition)
    {
        grid = FindObjectOfType<Grid>();
        Vector3 worldPosition = grid.GetCellCenterWorld(gridPosition);
        this.transform.position = new Vector3(worldPosition.x, 0, worldPosition.z);
    }
    
    public void GoDownStairs()
    {
        if (!turnManager.CanMove(this))
        {
            return;
        }
        if (this.gridPosition == testMap.GetGridPositionFromCell(testMap.somewhatInterestingMap.end))
        {
            gameStateManager.WinGame();
            turnManager.KickToBackOfTurnOrder(this);
        }
    }

    /// <summary>
    /// Takes in two locations, finds a path between them, then moves the Actor 1 step towards that location. Does not diagonal-check corners.
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <param name="targetPosition"></param>
    public void MoveToward(Vector3Int currentPosition, Vector3Int targetPosition)
    {
        if (currentPosition == targetPosition)
        {
            return;
        }

        ICell currentLocation = testMap.GetCellFromGridPosition(currentPosition);
        ICell targetLocation = testMap.GetCellFromGridPosition(targetPosition);
        Path newPath = testMap.pathFinder.TryFindShortestPath(currentLocation, targetLocation); //Determine the path between the two.
        if(newPath != null && newPath.Length > 1)
        {
            ICell nextStep = newPath?.StepForward();//Get the next step in that path.
            Vector3Int stepConv = testMap.GetGridPositionFromCell(nextStep);
            Move(stepConv - currentPosition);//Move that direction.
            turnManager.KickToBackOfTurnOrder(this);
        }
    }

    public bool CanSeePosition(Vector3Int position)
    {
        return testMap.CanSeePosition(gridPosition, position);
    }
}
