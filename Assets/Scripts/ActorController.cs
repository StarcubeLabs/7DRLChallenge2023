using PlasticPipe.PlasticProtocol.Messages;
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
    private TurnAnimationController turnAnimationController;
    private MoveRegistry moveRegistry;
    public Animator ActorAnimController;

    public Inventory Inventory;
    
    [SerializeField]
    private string displayName;

    [Tooltip("Hitpoint value range. X is the starting hp value, and Y is the maximum hp value.")]
    public Vector2Int hitPoints;


    [Tooltip("Hunger value range. X is the starting hunger value, and Y is the maximum hunger value.")]
    public Vector2Int hunger;
    public int starvationDamage = 1;

    [HideInInspector]
    public int visualHitPoints;
    [HideInInspector]
    public int visualHungerPoints;
    [HideInInspector]
    public bool Dead { get { return hitPoints.x <= 0; } }

    [SerializeField]
    private ElementType elementType;

    [Tooltip("Amount of damage the actor will deal without weapons.")]
    public int baseAttackPower;

    [SerializeField]
    private List<MoveData> startingMoves;

    //[HideInInspector]
    public List<Move> moves = new List<Move>();
    [HideInInspector]
    public Move moveToReplace;

    public Move moveToBeTaught;

    public bool IsMovesetFull { get { return moves.Count >= MAX_MOVES; } }

    [Header("Status Variables")]
    [Tooltip("The status that the actor is currently afflicted with.")]
    public StatusType afflictedStatus = StatusType.None;
    [Tooltip("How many turns left that the actor is afflicted with the current status.")]
    public int statusCountdown = 0;

    [SerializeField]
    private StatusType currentModifier = StatusType.None;
    private int modifierCountdown = 0;
    
    private StatusIcon statusIcon;

    [Header("Equippables")]
    private Item weapon;
    public BaseWeapon Weapon { get { return weapon == null ? null : (BaseWeapon)weapon.ItemData; }}
    private Item armor;
    public BaseArmor Armor { get { return armor == null ? null : (BaseArmor)armor.ItemData; }}
    private Item accessory;
    public EquippableItem Accessory { get { return accessory == null ? null : (EquippableItem)accessory.ItemData; }}

    /// <summary>
    /// Used by Status Effects to decide when their affects should be triggered. Counts up.
    /// </summary>
    /// 
    [HideInInspector]
    public int internalStatusCountdown = 0;

    /// <summary>
    /// Distinct from status effect countdown. Used to determine always on effects like hunger.
    /// </summary>
    /// 
    [HideInInspector]
    public int turnsTaken = 0;

    /// <summary>
    /// Determines whether an actor is immobilized by a status like Sleep or Petrify.
    /// </summary>
    /// 
    [HideInInspector]
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
        turnAnimationController = FindObjectOfType<TurnAnimationController>();
        moveRegistry = FindObjectOfType<MoveRegistry>();
        entityManager.AddActor(this);

        ActorAnimController = GetComponentInChildren<Animator>();
        statusIcon = GetComponentInChildren<StatusIcon>();

        gridPosition = grid.WorldToCell(this.transform.position);
        InitializePosition();

        visualHitPoints = hitPoints.x;
        visualHungerPoints = hunger.x;

        foreach (MoveData moveData in startingMoves)
        {
            AddMove(CreateMove(moveData));
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

    public bool UpdateVisualLocation()
    {
        bool animationFinished;
        Vector3 worldPosition = GetCellCenterWorld(gridPosition);
        if(Vector3.Distance(worldPosition, visualPosition) < .1f)
        {
            visualPosition = worldPosition;
            animationFinished = true;
        }
        else
        {
            visualPosition = Vector3.Lerp(visualPosition, new Vector3(worldPosition.x, 0, worldPosition.z), .25f);
            animationFinished = false;
        }
        this.transform.position = visualPosition;
        UpdateVisualRotation();
        return animationFinished;
    }

    public void UpdateVisualRotation()
    {
        visualTransform.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        visualTransform.transform.Rotate(0, 0, visualRotation);
    }

    public void InitializePosition()
    {
        SnapToPosition(gridPosition);
        visualPosition = GetCellCenterWorld(gridPosition);//Set visual position to grid position.
        this.transform.position = visualPosition;
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
        
        FaceDirection(offset);

        bool isMovingDiagonally = (offset.x * offset.y) != 0;

        if (isMovingDiagonally && !IsLegalDiagonalMove(offset) ||
            !levelManager.GetActiveLevel().CanWalkOnCell(gridPosition + offset))
        {
            UpdateVisualRotation();
            return;
        }

        gridPosition += offset;

        if (entityManager.isTrapInPosition(gridPosition))
        {
            var trap = entityManager.getTrapInPosition(gridPosition);
            trap.OnContact(this);
            trap.gameObject.SetActive(false);
        }

        IInteractable interactable = entityManager.getInteractableInPosition(gridPosition);
        interactable?.Interact(this);
        
        turnAnimationController.AddAnimation(new WalkAnimation(this, ActorAnimController));

        EndTurn();
    }

    public void EndTurn()
    { 
        if (turnManager.CanMove(this))
        {
            turnManager.KickToBackOfTurnOrder(this);
            TickStatus();
        }
    }

    public bool AddMove(Move move)
    {
        if (moves.Count >= MAX_MOVES)
        {
            return false;
        }
        moves.Add(move);
        return true;
    }

    public void ReplaceMove()
    {
        int moveIndexToReplace = moves.IndexOf(moveToReplace);
        moves[moveIndexToReplace] = moveToBeTaught;
        Destroy(moveToReplace.gameObject);
    }

    private Move CreateMove(MoveData moveData)
    {
        Move move = global::Move.InitiateFromMoveData(moveData);
        move.transform.parent = transform;
        return move;
    }

    public Move StartTeachMove(MoveData moveData)
    {
        moveToBeTaught = CreateMove(moveData);
        return moveToBeTaught;
    }

    public void EndTeachMove()
    {
        moveToReplace = null;
        moveToBeTaught = null;
    }

    public void UseBasicAttack()
    {
        UseMove(moveRegistry.BasicAttack);
    }

    public void UseMove(Move move)
    {
        if (move != moveRegistry.BasicAttack)
        {
            turnAnimationController.AddAnimation(new MessageAnimation($"{GetDisplayName()} used {move.moveData.MoveName}!"));
        }
        turnAnimationController.AddAnimation(new AnimatorAnimation(ActorAnimController, "Attack", UpdateVisualRotation));
        move.moveData.UseMove(this);
        if (move.pp > 0)
        {
            move.pp--;
        }
        EndTurn();
    }

    public int ModifyDamageUser(int damage, ActorController target)
    {
        foreach (EquippableItem equippableItem in GetEquippedItems())
        {
            damage = equippableItem.ModifyDamageUser(damage, this, target);
        }
        return damage;
    }

    public int ModifyDamageTarget(int damage, ActorController user)
    {
        foreach (EquippableItem equippableItem in GetEquippedItems())
        {
            damage = equippableItem.ModifyDamageTarget(damage, user, this);
        }
        return damage;
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
        turnAnimationController.AddAnimation(new MessageAnimation($"{GetDisplayName()} healed {healAmount} HP!"));
        hitPoints.x += healAmount;
        if (hitPoints.x > hitPoints.y)
        {
            hitPoints.x = hitPoints.y;
        }
        UpdateVisualHitPoints();
    }

    public void PowerRestore(int restoreAmount, string ppRestoreMessage = null)
    {
        if (ppRestoreMessage != null)
        {
            if(restoreAmount < 50)
            {
                turnAnimationController.AddAnimation(new MessageAnimation($"{GetDisplayName()} restored {restoreAmount} points worth of Combat Points!"));
            }
            else
            {
                turnAnimationController.AddAnimation(new MessageAnimation($"{GetDisplayName()} restored all combat points!"));
            }
        }
        foreach (Move move in moves)
        {
            print(move.name);
            move.RestorePP(restoreAmount);
        }
    }

    public void AddFood(int foodAmount, string foodMessage = null)
    {
        //Some things add food points as a side so only log it when it's needed
        if (foodMessage != null)
        {
            turnAnimationController.AddAnimation(new MessageAnimation($"{GetDisplayName()} ate {foodAmount} points worth of food!"));
        }
        hunger.x += foodAmount;
        UpdateVisualHitPoints();
    }

    public void DamageTarget(MoveData moveData, ActorController target)
    {
        int damage = DamageCalculator.CalculateDamage(moveData, this, target);
        if (damage > 0)
        {
            target.Hurt(damage);
            foreach (EquippableItem equippableItem in GetEquippedItems())
            {
                equippableItem.OnDamageDealt(this, target);
            }
        }
    }

    public void Hurt(int hurtAmount = 1, string hurtMessage = null)
    {
        hitPoints.x -= hurtAmount;
        hitPoints.x = Mathf.Max(0, hitPoints.x);
        if (hurtMessage == null)
        {
            hurtMessage = $"{GetDisplayName()} took {hurtAmount} damage!";
        }
        MessageAnimation hurtAnimation = new MessageAnimation(hurtMessage);
        turnAnimationController.AddAnimation(hurtAnimation);
        UpdateVisualHitPoints();
        if (hitPoints.x <= 0)
        {
            Kill();
        }
        else
        {
            turnAnimationController.AddAnimation(new AnimatorAnimation(ActorAnimController, "Hurt"));
        }
    }

    public void Kill()
    {
        turnAnimationController.AddAnimation(new MessageAnimation($"{GetDisplayName()} was defeated!"));
        turnAnimationController.AddAnimation(new DeathAnimation(this, ActorAnimController, "Die", onDie));
    }

    public void ApplyStatus(StatusType statusType, int turnCount)
    {
        bool allowStatus = true;
        foreach (EquippableItem equippableItem in GetEquippedItems())
        {
            if (!equippableItem.AllowStatus(this, statusType))
            {
                allowStatus = false;
                break;
            }
        }

        if (allowStatus)
        {
            afflictedStatus = statusType;
            statusCountdown = turnCount;
            List<StatusType> statusesList = new List<StatusType>();
            statusesList.Add(statusType);
            turnAnimationController.AddAnimation(new StatusAnimation(statusIcon, statusesList));
        }
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
            UpdateStatusIcons();
        }

        //We'll assume if we have an upper limit on hunger, we care about hunger. Otherwise who caresssss?
        if(hunger.y > 0)
        {
            if ((turnsTaken % 10) == 0)
            {
                if (hunger.x > 0 && turnsTaken > 0)
                {
                    hunger.x--;
                    UpdateVisualHunger();
                }
                if (hunger.x == 0)
                {
                    Hurt(starvationDamage, $"{GetDisplayName()} took {starvationDamage} damage from starvation!");
                }
            }
        }

        turnsTaken++;

        TickModifiers();
    }

    public void TickModifiers()
    {
        modifierCountdown--;
        if(modifierCountdown == 0)
        {
            currentModifier = StatusType.None;
        }
    }

    private void UpdateStatusIcons()
    {
        List<StatusType> statusesList = new List<StatusType>();
        if (afflictedStatus != StatusType.None)
        {
            statusesList.Add(afflictedStatus);
        }
        turnAnimationController.AddAnimation(new StatusAnimation(statusIcon, statusesList));
    }

    public void ApplyModifier(StatusType statusModifier, int turnCount) 
    {
        currentModifier = statusModifier;
        modifierCountdown = turnCount;
    }
    
    public void SnapToPosition(Vector3Int gridPosition)
    {
        grid = FindObjectOfType<Grid>();

        //Disable pure location snap so "faux-animation" lerping can work instead.
        this.transform.position = GetCellCenterWorld(gridPosition);
    }

    public Vector3 GetCellCenterWorld(Vector3Int gridPosition)
    {
        Vector3 worldPosition = grid.GetCellCenterWorld(gridPosition);
        worldPosition.y = 0;
        return worldPosition;
    }
    
    public void GoDownStairs()
    {
        if (this.gridPosition == levelManager.GetActiveLevel().GetGridPositionFromCell(levelManager.GetActiveLevel().somewhatInterestingMap.end))
        {
            levelManager.GoDownFloor();
            turnAnimationController.AddAnimation(new WalkAnimation(this, ActorAnimController));
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
        DisplayEquipMessage(weapon);
        this.weapon = weapon;
    }

    public void UnequipWeapon(Item weapon)
    {
        if (weapon == this.weapon)
        {
            this.weapon = null;
        }
    }

    public void EquipArmor(Item armor)
    {
        DisplayEquipMessage(armor);
        this.armor = armor;
    }

    public void UnequipArmor(Item armor)
    {
        if (armor == this.armor)
        {
            this.armor = null;
        }
    }

    public void EquipAccessory(Item accessory)
    {
        DisplayEquipMessage(this.accessory);
        this.accessory = accessory;
    }

    private void DisplayEquipMessage(Item equippedItem)
    {
        turnAnimationController.AddAnimation(new MessageAnimation($"{GetDisplayName()} equipped the {equippedItem.ItemName}!"));
    }

    public void UnequipAccessory(Item accessory)
    {
        if (accessory == this.accessory)
        {
            this.accessory = null;
        }
    }

    public List<EquippableItem> GetEquippedItems()
    {
        List<EquippableItem> equippableItems = new List<EquippableItem>();
        if (weapon)
        {
            equippableItems.Add((EquippableItem)weapon.ItemData);
        }
        if (armor)
        {
            equippableItems.Add((EquippableItem)armor.ItemData);
        }
        if (accessory)
        {
            equippableItems.Add((EquippableItem)accessory.ItemData);
        }
        return equippableItems;
    }

    public ElementType GetEffectiveType()
    {
        ElementType effectiveType = elementType;
        foreach (EquippableItem equippableItem in GetEquippedItems())
        {
            effectiveType = equippableItem.ModifyTypeTarget(effectiveType, this);
        }

        return effectiveType;
    }

    public void PlayEatAnimation()
    {
        turnAnimationController.AddAnimation(new AnimatorAnimation(ActorAnimController, "Eat"));
    }

    public void UpdateVisualHitPoints()
    {
        turnAnimationController.AddAnimation(new UpdateHitPoints(this, hitPoints.x));
    }

    public void UpdateVisualHunger()
    {
        turnAnimationController.AddAnimation(new UpdateHunger(this, hunger.x));
    }

    public string GetDisplayName()
    {
        return displayName;
    }
}
