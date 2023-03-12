using RLDataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;


public class ActorController : EntityController
{
    private const int MAX_MOVES = 4;

    public AudioSource audioSource;

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
    public ActorSoundContainer actorSoundContainer;
    
    [SerializeField]
    private string displayName;

    [Tooltip("Hitpoint value range. X is the starting hp value, and Y is the maximum hp value.")]
    public Vector2Int hitPoints;
    public bool HasFullHealth { get { return hitPoints.x >= hitPoints.y; } }

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
    private bool canPickUpItems;

    [SerializeField]
    private ElementType elementType;

    [Tooltip("Amount of damage the actor will deal without weapons.")]
    public int baseAttackPower;

    [SerializeField]
    private List<MoveData> startingMoves;

    [HideInInspector]
    public List<Move> moves = new List<Move>();
    [HideInInspector]
    public Move moveToReplace;
    [HideInInspector]
    public Move moveToBeTaught;

    public bool IsMovesetFull { get { return moves.Count >= MAX_MOVES; } }

    public List<Status> statuses = new List<Status>();
    /// <summary> Use the Statuses property for iteration to allow removing from the statuses list during iteration. </summary>
    public List<Status> Statuses { get { return new List<Status>(statuses); } }
    private StatusIcon statusIcon;

    [Header("Equippables")]
    private Item weapon;
    public BaseWeapon Weapon { get { return weapon == null ? null : (BaseWeapon)weapon.ItemData; }}
    private Item armor;
    public BaseArmor Armor { get { return armor == null ? null : (BaseArmor)armor.ItemData; }}
    private Item accessory;
    public EquippableItem Accessory { get { return accessory == null ? null : (EquippableItem)accessory.ItemData; }}

    /// <summary>
    /// Distinct from status effect countdown. Used to determine always on effects like hunger.
    /// </summary>
    [HideInInspector]
    public int turnsTaken = 0;

    public EventHandler onDie;
    public ParticleSystem onDieVFX;

    public enum ActorDirection { up, upRight, right, downRight, down, downLeft, left, upLeft, numDirections }

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
        actorSoundContainer = GetComponentInChildren<ActorSoundContainer>();

        gridPosition = grid.WorldToCell(this.transform.position);
        InitializePosition();

        visualHitPoints = hitPoints.x;
        visualHungerPoints = hunger.x;


        if (startingMoves.Count <= MAX_MOVES)
        {
            foreach (MoveData moveData in startingMoves)
            {
                AddMove(CreateMove(moveData));
            }
        }
        else
        {
            for (int i = 0; i < MAX_MOVES; i++)
            {
                int randomIndex = Random.Range(0, startingMoves.Count);
                AddMove(CreateMove(startingMoves[randomIndex]));
                startingMoves.RemoveAt(randomIndex);
            }
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

    public bool UpdateVisualLocation(float animationTime)
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
            visualPosition = Vector3.Lerp(visualPosition, new Vector3(worldPosition.x, 0, worldPosition.z), animationTime);
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
    public void Walk(Vector3Int offset)
    {
        //If we are waiting, just skip.
        if (offset == Vector3Int.zero)
        {
            EndTurn();
            return;
        }
        
        FaceDirection(offset);
        
        Statuses.ForEach(status => status.ModifyFacingDirection());
        
        offset = GetOffsetFromDirection();

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

        if (canPickUpItems)
        {
            IInteractable interactable = entityManager.getInteractableInPosition(gridPosition);
            interactable?.Interact(this);
        }
        
        turnAnimationController.AddAnimation(new WalkAnimation(this, ActorAnimController));

        EndTurn();
    }

    public void EndTurn(bool force = false)
    { 
        if (force || turnManager.CanMove(this))
        {
            turnManager.KickToBackOfTurnOrder(this);
            TickStatus();
        }
    }

    public void ForceLocation(Vector3Int position)
    {
        gridPosition = position;
        turnAnimationController.AddAnimation(new ForceLocationAnimation(this, ActorAnimController));
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

    public bool IsMoveUsable(Move move)
    {
        return move.IsMoveUsable() && Statuses.All(status => status.IsMoveUsable(move));
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
        Statuses.ForEach(status => status.ModifyFacingDirection());
        
        void TriggerMoveVFX()
        {
            UpdateVisualRotation();
            move.moveData.TriggerUserVFX(this);
        }
        
        turnAnimationController.AddAnimation(new AnimatorAnimation(ActorAnimController, "Attack", TriggerMoveVFX));
        move.moveData.UseMove(this);
        if (move.pp > 0)
        {
            move.pp--;
        }
        EndTurn();
    }

    public void TriggerDeathVFX()
    {
        if (onDieVFX != null)
        {
            ParticleSystem ps = GameObject.Instantiate<ParticleSystem>(onDieVFX, this.transform, false);
            ps.transform.parent = null;
        }
    }

    public int ModifyDamageUser(int damage, ActorController target)
    {
        foreach (EquippableItem equippableItem in GetEquippedItems())
        {
            damage = equippableItem.ModifyDamageUser(damage, this, target);
        }

        foreach (Status status in Statuses)
        {
            damage = status.ModifyDamageUser(damage, target);
        }
        return damage;
    }

    public int ModifyDamageTarget(int damage, ActorController user)
    {
        foreach (EquippableItem equippableItem in GetEquippedItems())
        {
            damage = equippableItem.ModifyDamageTarget(damage, user, this);
        }

        foreach (Status status in Statuses)
        {
            damage = status.ModifyDamageTarget(damage, user);
        }
        return damage;
    }

    public void FaceDirection(Vector3Int offset)
    {
        ActorDirection direction;
        //Big 'ol If Chain to determine character facing direction based on movement direction
        if (offset == Vector3Int.up)
        {
            direction = ActorDirection.up;
        }
        else if (offset == new Vector3Int(1, 1))
        {
            direction = ActorDirection.upRight;
        }
        else if (offset == Vector3Int.right)
        {
            direction = ActorDirection.right;
        }
        else if (offset == new Vector3Int(1, -1))
        {
            direction = ActorDirection.downRight;
        }
        else if (offset == Vector3Int.down)
        {
            direction = ActorDirection.down;
        }
        else if (offset == new Vector3Int(-1, -1))
        {
            direction = ActorDirection.downLeft;
        }
        else if (offset == Vector3Int.left)
        {
            direction = ActorDirection.left;
        }
        else if (offset == new Vector3Int(-1, 1))
        {
            direction = ActorDirection.upLeft;
        }
        else
        {
            direction = ActorDirection.down;
        }
        FaceDirection(direction);
    }

    public void FaceDirection(ActorDirection direction)
    {
        actorDirection = direction;
        //Big 'ol If Chain to determine character facing direction based on movement direction
        switch (direction)
        {
            case ActorDirection.up:
                visualRotation = 180;
                break;
            case ActorDirection.upRight:
                visualRotation = 135;
                break;
            case ActorDirection.right:
                visualRotation = 90;
                break;
            case ActorDirection.downRight:
                visualRotation = 45;
                break;
            case ActorDirection.down:
                visualRotation = 0;
                break;
            case ActorDirection.downLeft:
                visualRotation = 315;
                break;
            case ActorDirection.left:
                visualRotation = 270;
                break;
            case ActorDirection.upLeft:
                visualRotation = 225;
                break;
        }
    }
    
    public Vector3Int GetOffsetFromDirection()
    {
        switch (actorDirection)
        {
            case ActorDirection.up: return Vector3Int.up;
            case ActorDirection.upRight: return new Vector3Int(1, 1);
            case ActorDirection.right: return Vector3Int.right;
            case ActorDirection.downRight: return new Vector3Int(1, -1);
            case ActorDirection.down: return Vector3Int.down;
            case ActorDirection.downLeft: return new Vector3Int(-1, -1);
            case ActorDirection.left: return Vector3Int.left;
            case ActorDirection.upLeft: return new Vector3Int(-1, 1);
            default: return Vector3Int.down;
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
        for(int i = 0; i < moves.Count; i++)
        {
            print(moves[i].name);
            moves[i].RestorePP(restoreAmount);
        }
    }

    public void AddFood(int foodAmount, string foodMessage = null)
    {
        //Some things add food points as a side so only log it when it's needed
        if (foodMessage != null)
        {
            turnAnimationController.AddAnimation(new MessageAnimation(foodMessage));
        }
        hunger.x += foodAmount;
        UpdateVisualHitPoints();
    }

    public int DamageTarget(MoveData moveData, ActorController target)
    {
        int damage = DamageCalculator.CalculateDamage(moveData, this, target);
        if (damage > 0)
        {
            target.Hurt(damage, moveData);
            GetEquippedItems().ForEach(item => item.OnDamageDealt(this, target));
            target.Statuses.ForEach(status => status.OnActorAttacked(this));
        }

        return damage;
    }

    public void Hurt(int hurtAmount = 1, MoveData moveData = null, string hurtMessage = null)
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
        void TriggerTargetVFX()
        {
            moveData?.TriggerTargetVFX(this);
        }
        if (hitPoints.x <= 0)
        {
            Kill(TriggerTargetVFX);
        }
        else
        {
            turnAnimationController.AddAnimation(new AnimatorAnimation(ActorAnimController, "Hurt", TriggerTargetVFX, actorSoundContainer?.hitSound));
        }
    }

    public void Kill(Action attackAnimation = null)
    {
        turnAnimationController.AddAnimation(new MessageAnimation($"{GetDisplayName()} was defeated!"));
        turnAnimationController.AddAnimation(new DeathAnimation(this, ActorAnimController, "Die", attackAnimation, onDie, actorSoundContainer?.dieSound));
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
            foreach (Status status in Statuses)
            {
                if (!status.AllowStatus(statusType))
                {
                    allowStatus = false;
                    break;
                }
            }
        }

        if (allowStatus)
        {
            Status newStatus = moveRegistry.CreateStatusFromType(statusType, this, turnCount);
            if (newStatus != null)
            {
                Status conflictingStatus = statuses.Find(status => moveRegistry.DoStatusesConflict(status.Type, newStatus.Type));
                if (conflictingStatus != null)
                {
                    statuses.Remove(conflictingStatus);
                }
                
                statuses.Add(newStatus);
                UpdateStatusIcons();
                string statusApplyMessage = newStatus.GetStatusApplyMessage();
                if (statusApplyMessage != null)
                {
                    turnAnimationController.AddAnimation(new MessageAnimation(statusApplyMessage));
                }
            }
        }
    }

    public bool HasStatus(StatusType status)
    {
        return statuses.Exists(s => s.Type == status);
    }

    public bool HasVisualStatus(StatusType status)
    {
        return statusIcon.HasStatus(status);
    }

    public void CureStatus(Status status)
    {
        statuses.Remove(status);
        turnAnimationController.AddAnimation(new MessageAnimation(status.GetStatusCureMessage()));
        UpdateStatusIcons();
    }

    public void CureStatus(StatusType status)
    {
        int statusIndex = statuses.FindIndex(s => s.Type == status);
        if (statusIndex >= 0)
        {
            CureStatus(statuses[statusIndex]);
        }
    }

    public void CureStatuses(params StatusType[] statusTypes)
    {
        statuses
            .FindAll(status => statusTypes.Contains(status.Type))
            .ForEach(CureStatus);
    }

    public void TickStatus()
    {
        foreach (Status status in Statuses)
        {
            if (status.TickStatus())
            {
                CureStatus(status);
            }
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
                    Hurt(starvationDamage, null, $"{GetDisplayName()} took {starvationDamage} damage from starvation!");
                }
            }
        }

        turnsTaken++;
    }

    private void UpdateStatusIcons()
    {
        List<StatusType> statusesList = statuses.ConvertAll(status => status.Type);
        turnAnimationController.AddAnimation(new StatusAnimation(statusIcon, statusesList));
    }

    public bool IsImmobilized()
    {
        return statuses.Any(status => status.IsImmobilized());
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
            turnAnimationController.AddAnimation(new WalkAnimation(this, ActorAnimController));
            levelManager.GoDownFloor();
            UpdateVisualLocation(1);
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
        DisplayEquipMessage(accessory);
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
