using RogueSharp;
using System.Collections.Generic;
using System.Linq;
using RLDataTypes;
using UnityEngine;

public enum AI_STATES
{
    WANDERING,
    PURSUING,
    ATTACKING,
    IDLE,
}

public class EnemyBaseScript : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float basicAttackChance = 0.5f;
    
    public ActorController enemyActor;
    public PlayerInputController player;
    public TurnManager turnManager;
    private LevelManager levelManager;
    private EntityManager entityManager;
    public AI_STATES state;

    // Start is called before the first frame update
    void Start()
    {
        enemyActor = this.gameObject.GetComponent<ActorController>();
        player = GameObject.FindObjectOfType<PlayerInputController>();
        turnManager = FindObjectOfType<TurnManager>();
        levelManager = FindObjectOfType<LevelManager>();
        entityManager = FindObjectOfType<EntityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnManager.CanMove(enemyActor) && !enemyActor.Dead && !player.playerActor.Dead)
        {
            if (state != AI_STATES.PURSUING && state != AI_STATES.ATTACKING && enemyActor.CanSeePosition(player.GetLocation()))
            {
                state = AI_STATES.PURSUING;
            }

            if (enemyActor.HasStatus(StatusType.Blindness))
            {
                state = AI_STATES.WANDERING;
            }

            switch (state)
            {
                case AI_STATES.WANDERING:
                    enemyActor.Walk(new Vector3Int(Random.Range(-1, 2), Random.Range(-1, 2)));
                    break;
                case AI_STATES.PURSUING:
                    MoveToward(enemyActor.gridPosition, player.GetLocation());
                    break;
                default:
                    break;
            }

            enemyActor.EndTurn();
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

        LevelController activeLevel = levelManager.GetActiveLevel();
        ICell currentLocation = activeLevel.GetCellFromGridPosition(currentPosition);
        ICell targetLocation = activeLevel.GetCellFromGridPosition(targetPosition);
        Path newPath = activeLevel.pathFinder.TryFindShortestPath(currentLocation, targetLocation); //Determine the path between the two.
        if(newPath != null && newPath.Length > 0)
        {
            ICell nextStep = newPath?.StepForward();//Get the next step in that path.
            Vector3Int stepConv = activeLevel.GetGridPositionFromCell(nextStep);
            Vector3Int toNextStep = stepConv - currentPosition;

            Vector3Int nextOffset = Vector3Int.zero;
            if ((toNextStep.x * toNextStep.y) != 0 && !enemyActor.IsLegalDiagonalMove(toNextStep))
            {
                if (activeLevel.CanWalkOnCell(currentPosition + new Vector3Int(toNextStep.x, 0)))
                {
                    nextOffset = new Vector3Int(toNextStep.x, 0);
                }
                else if(activeLevel.CanWalkOnCell(currentPosition + new Vector3Int(0, toNextStep.y)))
                {
                    nextOffset = new Vector3Int(0, toNextStep.y);
                }
            }
            else
            {
                nextOffset = toNextStep;
            }
            
            enemyActor.FaceDirection(nextOffset);

            if (!TryAttack(currentPosition, nextOffset))
            {
                enemyActor.Walk(nextOffset);//Move that direction.
            }
        }
    }

    public bool TryAttack(Vector3Int currentPosition, Vector3Int nextOffset)
    {
        ActorController playerActor = player.GetComponent<ActorController>();
        List<Move> usableMoves =
            enemyActor.moves.FindAll(move => enemyActor.IsMoveUsable(move) && move.moveData.UsableByAI(enemyActor, playerActor));
        
        
        if (usableMoves.Count == 0 || Random.value <= basicAttackChance)
        {
            if (ServicesManager.MoveRegistry.BasicAttack.moveData.InAIRange(enemyActor, playerActor))
            {
                enemyActor.UseBasicAttack();
                return true;
            }
        }
        else
        {
            Move chosenMove = usableMoves[Random.Range(0, usableMoves.Count)];
            if (chosenMove.moveData.InAIRange(enemyActor, playerActor))
            {
                enemyActor.UseMove(chosenMove);
                return true;
            }
        }

        return false;
    }
}
