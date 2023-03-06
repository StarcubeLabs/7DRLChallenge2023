using RLDataTypes;
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
    [HideInInspector]
    public Vector3 visualPosition;
    [HideInInspector]
    public float visualRotation = 0;
    public Transform visualTransform;
    TestMap testMap;

    GameStateManager gameStateManager;
    EntityManager entityManager;
    TurnManager turnManager;

    public Inventory Inventory;

    [Tooltip("Hitpoint value range. X is the starting hp value, and Y is the maximum hp value.")]
    public Vector2Int hitPoints;

    [Tooltip("The status that the actor is currently afflicted with.")]
    public StatusType afflictedStatus = StatusType.None;
    [Tooltip("How many turns left that the actor is afflicted with the current status.")]
    public int statusCountdown = 0;

    public EventHandler onDie;

    public enum ActorDirection { up, upRight, right, downRight, down, downLeft, left, upLeft}

    public ActorDirection actorDirection = ActorDirection.down;

    private void Awake()
    {
        Inventory = new Inventory();
        Inventory.InitializeInventory(30);
    }

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
        visualPosition = (Vector3)gridPosition;//Set visual position to grid position.
    }

    private void OnDestroy()
    {
        entityManager.RemoveActor(this);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisualLocation();
    }

    void UpdateVisualLocation()
    {
        Vector3 worldPosition = grid.GetCellCenterWorld(gridPosition);
        if(Vector3.Distance(worldPosition, visualPosition) < .1f)
        {
            visualPosition = gridPosition;
        }
        else
        {
            visualPosition = Vector3.Lerp(visualPosition, new Vector3(worldPosition.x, 0, worldPosition.z), .25f);
        }
        this.transform.position = visualPosition;
        //visualTransform.transform.rotation = Quaternion.Euler(visualRotation);
        visualTransform.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        visualTransform.transform.Rotate(0, 0, visualRotation);
    }

    /// <summary>
    /// Moves an Actor. Also considers diagonals to prevent corner cutting of solid or filled in grid squares.
    /// </summary>
    /// <param name="offset">The direction of the square adjacent to the Actor to move in.</param>
    public void Move(Vector3Int offset)
    {
        if (offset == Vector3Int.zero)
        {
            turnManager.KickToBackOfTurnOrder(this);
            return;
        }
        if (!turnManager.CanMove(this))
        {
            return;
        }


        bool isMovingDialogonally = (offset.x * offset.y) != 0;

        if (!isMovingDialogonally || IsLegalDiagonalMove(offset))
        {
            ActorController entityToAttack = entityManager.getEntityInPosition(gridPosition + offset);
            if (entityToAttack != null && entityToAttack != this)
            {
                entityToAttack.Hurt();
                turnManager.KickToBackOfTurnOrder(this);
                return;
            }

            if (!testMap.CanWalkOnCell(gridPosition + offset))
            {
                return;
            }

            //Big 'ol If Chain to determine character facing direction based on movement direction
            if(offset == Vector3Int.up)
            {
                actorDirection = ActorDirection.up;
                visualRotation = 180;
            }
            else if(offset == new Vector3Int(1,1, 0))
            {
                actorDirection = ActorDirection.upRight;
                visualRotation = 135;
            }
            else if (offset == Vector3Int.right)
            {
                actorDirection = ActorDirection.right;
                visualRotation = 90;
            }
            else if (offset == new Vector3Int(1, -1, 0))
            {
                actorDirection = ActorDirection.downRight;
                visualRotation = 45;
            }
            else if (offset == Vector3Int.down)
            {
                actorDirection = ActorDirection.down;
                visualRotation = 0;
            }
            else if (offset == new Vector3Int(-1, -1, 0))
            {
                actorDirection = ActorDirection.downLeft;
                visualRotation = 315;
            }
            else if (offset == Vector3Int.left)
            {
                actorDirection = ActorDirection.left;
                visualRotation = 270;
            }
            else if (offset == new Vector3Int(-1, 1, 0))
            {
                actorDirection = ActorDirection.upLeft;
                visualRotation = 225;
            }
            else 
            {
                actorDirection = ActorDirection.down;
                visualRotation = 0;
            }



            gridPosition += offset;
            SnapToPosition(gridPosition);

            if (entityManager.isTrapInPosition(gridPosition))
            {
                var trap = entityManager.getTrapInPosition(gridPosition);
                trap.OnContact(this);
                trap.gameObject.SetActive(false);
            }
            
            turnManager.KickToBackOfTurnOrder(this);
        }
    }

    public bool IsLegalDiagonalMove(Vector3Int offset)
    {
        return testMap.CanWalkOnCell(gridPosition + new Vector3Int(offset.x, 0, 0)) &&
               testMap.CanWalkOnCell(gridPosition + new Vector3Int(0, offset.y, 0));
    }

    public void HealAmount(int healAmount)
    {
        hitPoints.x += healAmount;
        if (hitPoints.x > hitPoints.y)
        {
            hitPoints.x = hitPoints.y;
        }
    }
    
    public void Hurt()
    {
        hitPoints.x--;
        if (hitPoints.x <= 0)
        {
            Kill();
        }
    }

    public void Hurt(int hurtAmount)
    {
        hitPoints.x -= hurtAmount;
        if (hitPoints.x <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        if(onDie != null)
        {
            onDie(this, EventArgs.Empty);
        }
        Destroy(this.gameObject);
    }

    public void ApplyStatus(StatusType statusType, int turnCount)
    {
        afflictedStatus = statusType;
        statusCountdown = turnCount;
    }

    public void SnapToPosition(Vector3Int gridPosition)
    {
        grid = FindObjectOfType<Grid>();

        //Disable pure location snap so "faux-animation" lerping can work instead.
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
        if(newPath != null && newPath.Length > 0)
        {
            ICell nextStep = newPath?.StepForward();//Get the next step in that path.
            Vector3Int stepConv = testMap.GetGridPositionFromCell(nextStep);
            Vector3Int toNextStep = stepConv - currentPosition;
            
            if ((toNextStep.x * toNextStep.y) != 0 && !IsLegalDiagonalMove(toNextStep))
            {
                if (testMap.CanWalkOnCell(currentPosition + new Vector3Int(toNextStep.x, 0)))
                {
                    Move(new Vector3Int(toNextStep.x, 0));
                }
                else if(testMap.CanWalkOnCell(currentPosition + new Vector3Int(0, toNextStep.y)))
                {
                    Move(new Vector3Int(0, toNextStep.y));
                }
            }
            else
            {
                Move(toNextStep);//Move that direction.
            }
            turnManager.KickToBackOfTurnOrder(this);
        }
    }

    public bool CanSeePosition(Vector3Int position)
    {
        return testMap.CanSeePosition(gridPosition, position);
    }

    public void Interact()
    {
        Debug.Log(gridPosition);
        if (entityManager.isInteractableInPosition(gridPosition))
        {
            Debug.Log("WE ARE HERE.");
            entityManager.getInteractableInPosition(gridPosition).Interact(this);
        }
    }
    
}
