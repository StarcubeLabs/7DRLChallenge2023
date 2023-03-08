using RLDataTypes;
using RogueSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorController : EntityController
{
    Grid grid;
    [HideInInspector]
    public Vector3 visualPosition;
    [HideInInspector]
    public float visualRotation = 0;
    public Transform visualTransform;

    LevelManager levelManager;
    EntityManager entityManager;
    TurnManager turnManager;
    public Animator ActorAnimController;

    public Inventory Inventory;

    [Tooltip("Hitpoint value range. X is the starting hp value, and Y is the maximum hp value.")]
    public Vector2Int hitPoints;

    [Tooltip("Amount of damage the actor will deal without weapons.")]
    public int baseAttackPower;

    public int AttackPower
    {
        get
        {
            if (weapon == null)
            {
                return baseAttackPower;
            }
            return baseAttackPower + weapon.power;
        }
    }

    [Header("Status Variables")]
    [Tooltip("The status that the actor is currently afflicted with.")]
    public StatusType afflictedStatus = StatusType.None;
    [Tooltip("How many turns left that the actor is afflicted with the current status.")]
    public int statusCountdown = 0;

    private WeaponItem weapon;

    /// <summary>
    /// Used by Status Effects to decide when their affects should be triggered. Counts up.
    /// </summary>
    public int internalStatusCountdown = 0;

    /// <summary>
    /// Determines whether an actor is immobilized by a status like Sleep or Petrify.
    /// </summary>
    public bool isStatusImmobilized = false;

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
        levelManager = FindObjectOfType<LevelManager>();
        turnManager = FindObjectOfType<TurnManager>();
        entityManager = FindObjectOfType<EntityManager>();
        entityManager.AddActor(this);

        gridPosition = grid.WorldToCell(this.transform.position);
        SnapToPosition(gridPosition);
        visualPosition = (Vector3)gridPosition;//Set visual position to grid position.
    }

    private void OnDestroy()
    {
        entityManager?.RemoveActor(this);
    }

    private void OnDisable()
    {
        if(entityManager != null)
        {
            entityManager.RemoveActor(this);
        }
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

        //We can't move at all, so kick us back to the turn line.
        if(isStatusImmobilized)
        {
            turnManager.KickToBackOfTurnOrder(this);
            TickStatus();
            return;
        }

        //We're confused. Set our direction to RANDOM.
        if (afflictedStatus == StatusType.Confusion)
        {
            offset.x = UnityEngine.Random.Range(-1, 2);
            offset.y = UnityEngine.Random.Range(-1, 2);
        }

        //If we are waiting, just skip.
        if (offset == Vector3Int.zero)
        {
            turnManager.KickToBackOfTurnOrder(this);
            TickStatus();
            return;
        }
        //If we tried to go somewhere that just won't work, try again.
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
                entityToAttack.Hurt(AttackPower);
                turnManager.KickToBackOfTurnOrder(this);
                FaceDirection(offset);
                if (ActorAnimController != null)
                {
                    ActorAnimController?.SetTrigger("Attack");
                }
                TickStatus();
                return;
            }

            if (!levelManager.GetActiveLevel().CanWalkOnCell(gridPosition + offset))
            {
                return;
            }

            //Big 'ol If Chain to determine character facing direction based on movement direction
            FaceDirection(offset);



            gridPosition += offset;
            SnapToPosition(gridPosition);
            if(ActorAnimController != null)
            {
                ActorAnimController?.SetTrigger("Walk");
            }

            if (entityManager.isTrapInPosition(gridPosition))
            {
                var trap = entityManager.getTrapInPosition(gridPosition);
                trap.OnContact(this);
                trap.gameObject.SetActive(false);
            }
            
            turnManager.KickToBackOfTurnOrder(this);
            TickStatus();
        }
    }

    public void FaceDirection(Vector3Int offset)
    {
        //Big 'ol If Chain to determine character facing direction based on movement direction
        if (offset == Vector3Int.up)
        {
            actorDirection = ActorDirection.up;
            visualRotation = 180;
        }
        else if (offset == new Vector3Int(1, 1, 0))
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
    }

    public bool IsLegalDiagonalMove(Vector3Int offset)
    {
        return levelManager.GetActiveLevel().CanWalkOnCell(gridPosition + new Vector3Int(offset.x, 0, 0)) &&
               levelManager.GetActiveLevel().CanWalkOnCell(gridPosition + new Vector3Int(0, offset.y, 0));
    }

    public void HealAmount(int healAmount)
    {
        hitPoints.x += healAmount;
        if (hitPoints.x > hitPoints.y)
        {
            hitPoints.x = hitPoints.y;
        }
    }

    public void Hurt(int hurtAmount = 1)
    {
        hitPoints.x -= hurtAmount;
        print("Hurt! Value: " + hurtAmount);
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

    public void TickStatus()
    {
        if (statusCountdown != 0)
        {
            switch (afflictedStatus)
            {
                case StatusType.Poison:
                    if (internalStatusCountdown == 2)
                    {
                        this.Hurt(1);
                        internalStatusCountdown = 0;
                    }

                    statusCountdown--;
                    internalStatusCountdown++;
                    break;

                case StatusType.Confusion:
                    statusCountdown--;
                    break;

                case StatusType.Sleep:
                    if(statusCountdown != 0)
                    {
                        isStatusImmobilized = true;
                        statusCountdown--;
                    }

                    
                    break;

                default:

                    break;
            }
        }
        //We've survived the status and should clear it.
        if(statusCountdown == 0)
        {
            afflictedStatus = StatusType.None;
            isStatusImmobilized = false;
        }
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
        if (this.gridPosition == levelManager.GetActiveLevel().GetGridPositionFromCell(levelManager.GetActiveLevel().somewhatInterestingMap.end))
        {
            levelManager.GoDownFloor();
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

        ICell currentLocation = levelManager.GetActiveLevel().GetCellFromGridPosition(currentPosition);
        ICell targetLocation = levelManager.GetActiveLevel().GetCellFromGridPosition(targetPosition);
        Path newPath = levelManager.GetActiveLevel().pathFinder.TryFindShortestPath(currentLocation, targetLocation); //Determine the path between the two.
        if(newPath != null && newPath.Length > 0)
        {
            ICell nextStep = newPath?.StepForward();//Get the next step in that path.
            Vector3Int stepConv = levelManager.GetActiveLevel().GetGridPositionFromCell(nextStep);
            Vector3Int toNextStep = stepConv - currentPosition;
            
            if ((toNextStep.x * toNextStep.y) != 0 && !IsLegalDiagonalMove(toNextStep))
            {
                if (levelManager.GetActiveLevel().CanWalkOnCell(currentPosition + new Vector3Int(toNextStep.x, 0)))
                {
                    Move(new Vector3Int(toNextStep.x, 0));
                }
                else if(levelManager.GetActiveLevel().CanWalkOnCell(currentPosition + new Vector3Int(0, toNextStep.y)))
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
        return levelManager.GetActiveLevel().CanSeePosition(gridPosition, position);
    }

    public void Interact()
    {
        if (entityManager.isInteractableInPosition(gridPosition))
        {
            entityManager.getInteractableInPosition(gridPosition).Interact(this);
        }
    }

    public void EquipWeapon(WeaponItem weapon)
    {
        this.weapon = weapon;
    }
}
