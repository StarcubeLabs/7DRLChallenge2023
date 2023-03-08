using RLDataTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : EntityController
{
    private const int MAX_MOVES = 4;
    
    Grid grid;
    [HideInInspector]
    public Vector3 visualPosition;
    [HideInInspector]
    public float visualRotation = 0;
    public Transform visualTransform;

    LevelManager levelManager;
    EntityManager entityManager;
    TurnManager turnManager;
    private MoveRegistry moveRegistry;
    public Animator ActorAnimController;

    public Inventory Inventory;

    [Tooltip("Hitpoint value range. X is the starting hp value, and Y is the maximum hp value.")]
    public Vector2Int hitPoints;

    [Tooltip("Amount of damage the actor will deal without weapons.")]
    public int baseAttackPower;

    public int AttackPower { get { return baseAttackPower + WeaponDamage; } }

    [SerializeField]
    private List<MoveData> startingMoves;
    public List<Move> moves = new List<Move>();

    [Header("Status Variables")]
    [Tooltip("The status that the actor is currently afflicted with.")]
    public StatusType afflictedStatus = StatusType.None;
    [Tooltip("How many turns left that the actor is afflicted with the current status.")]
    public int statusCountdown = 0;

    private Item weapon;
    private int WeaponDamage { get { return weapon == null ? 0 : ((WeaponItem)weapon.ItemData).power; } }

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
        moveRegistry = FindObjectOfType<MoveRegistry>();
        entityManager.AddActor(this);

        ActorAnimController = GetComponentInChildren<Animator>();

        gridPosition = grid.WorldToCell(this.transform.position);
        SnapToPosition(gridPosition);
        visualPosition = (Vector3)gridPosition;//Set visual position to grid position.

        foreach (MoveData moveData in startingMoves)
        {
            AddMove(moveData);
        }
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
        //If we tried to go somewhere that just won't work, try again.
        if (!turnManager.CanMove(this))
        {
            return;
        }

        //We can't move at all, so kick us back to the turn line.
        if(isStatusImmobilized)
        {
            EndTurn();
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
            EndTurn();
            return;
        }

        bool isMovingDiagonally = (offset.x * offset.y) != 0;

        if (!isMovingDiagonally || IsLegalDiagonalMove(offset))
        {
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

            IInteractable interactable = entityManager.getInteractableInPosition(gridPosition);
            interactable?.Interact(this);

            EndTurn();
        }
    }

    public void EndTurn()
    { 
        if (turnManager.CanMove(this))
        {
            turnManager.KickToBackOfTurnOrder(this);
            TickStatus();
        }
    }

    public bool AddMove(MoveData moveData)
    {
        if (moves.Count >= MAX_MOVES)
        {
            return false;
        }
        moves.Add(global::Move.InitiateFromMoveData(moveData));
        return true;
    }

    public void ReplaceMove(int moveIndex, MoveData moveData)
    {
        moves[moveIndex] = global::Move.InitiateFromMoveData(moveData);
    }

    public void UseBasicAttack()
    {
        UseMove(moveRegistry.BasicAttack);
    }

    public void UseMove(Move move)
    {
        if (!turnManager.CanMove(this))
        {
            return;
        }
        
        move.moveData.UseMove(this, entityManager);
        if (ActorAnimController != null)
        {
            ActorAnimController?.SetTrigger("Attack");
        }
        EndTurn();
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

    public Vector3Int GetPositionInFront(int numTiles = 1)
    {
        Vector3Int directionOffset = Vector3Int.zero;
        switch (actorDirection)
        {
            case ActorDirection.up:
                directionOffset = Vector3Int.up;
                break;
            case ActorDirection.upRight:
                directionOffset = new Vector3Int(1, 1);
                break;

            case ActorDirection.right:
                directionOffset = Vector3Int.right;
                break;

            case ActorDirection.downRight:
                directionOffset = new Vector3Int(1, -1);
                break;

            case ActorDirection.down:
                directionOffset = Vector3Int.down;
                break;

            case ActorDirection.downLeft:
                directionOffset = new Vector3Int(-1, -1);
                break;

            case ActorDirection.left:
                directionOffset = Vector3Int.left;
                break;

            case ActorDirection.upLeft:
                directionOffset = new Vector3Int(-1, 1);
                break;
        }
        return directionOffset * numTiles + gridPosition;
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

                case StatusType.Slow:
                    if (internalStatusCountdown != 2)
                    {
                        isStatusImmobilized = true;
                        internalStatusCountdown++;
                        statusCountdown--;
                    }
                    else
                    {
                        isStatusImmobilized = false;
                        internalStatusCountdown = 0;
                    }

                    break;

                case StatusType.Petrify:
                    if (statusCountdown != 0)
                    {
                        isStatusImmobilized = true;
                        statusCountdown--;
                    }


                    break;

                case StatusType.Muteness:

                    statusCountdown--;

                    break;

                case StatusType.Blindness:

                    statusCountdown--;

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

    public void EquipWeapon(Item weapon)
    {
        this.weapon = weapon;
    }

    public void UnequipWeapon(Item weapon)
    {
        if (weapon == this.weapon)
        {
            this.weapon = null;
        }
    }
}
