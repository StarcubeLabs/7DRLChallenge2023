using RogueSharp;
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

            switch (state)
            {
                case AI_STATES.WANDERING:
                    enemyActor.Move(new Vector3Int(Random.Range(-1, 2), Random.Range(-1, 2)));
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

            ActorController entityInFront = entityManager.getEntityInPosition(currentPosition + nextOffset);
            if (entityInFront == player.GetComponent<ActorController>())
            {
                enemyActor.UseBasicAttack();
            }
            else
            {
                enemyActor.Move(nextOffset);//Move that direction.
            }
        }
    }
}
